using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CapitalNerd.Laranjinhai.Application.Interfaces;
using System.Text.Json;
using MediatR;
using CapitalNerd.Laranjinhai.Application.Commands;
using CapitalNerd.Laranjinhai.Application.Dtos;
using System.Net.Http;

namespace CapitalNerd.Laranjinhai.API.Controllers;

[ApiController]
[Route("webhook")]
public class WebhookController : ControllerBase
{
    private readonly ILogger<WebhookController> _logger;
    private readonly IMediator _mediator;
    private readonly IHttpClientFactory _httpClientFactory;

    public WebhookController(
        ILogger<WebhookController> logger,
        IMediator mediator,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _mediator = mediator;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] JsonElement body)
    {
        _logger.LogInformation("📩 Webhook recebido do WhatsConnect: {Body}", body.ToString());

        // Ignora chamadas de validação/teste que não contêm mensagens reais
        if (!body.TryGetProperty("event", out var evt) || evt.GetString() != "onmessage")
        {
            _logger.LogInformation("🚦 Evento ignorado. Tipo: {Tipo}", evt.GetString());
            return Ok(new { status = "ignorado" });
        }

        try
        {
            var dto = new WebhookMessageDto
            {
                From = body.GetProperty("from").GetString() ?? string.Empty,
                To = body.GetProperty("to").GetString() ?? string.Empty,
                Body = body.GetProperty("body").GetString() ?? string.Empty,
                Type = body.GetProperty("type").GetString() ?? string.Empty,
                NotifyName = body.TryGetProperty("notifyName", out var notifyName) ? notifyName.GetString() : null,
                RawJson = body
            };

            if (dto is null)
            {
                _logger.LogWarning("❌ Payload inválido.");
                return BadRequest("Formato inválido.");
            }

            var command = new ProcessarMensagemCommand(dto);
            await _mediator.Send(command);

            return Ok(new { status = "mensagem processada com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao processar e salvar mensagem recebida.");
            return StatusCode(500, "Erro ao processar mensagem.");
        }
    }

    [HttpGet("qrcode")]
    public async Task<IActionResult> GetQrCode([FromQuery] string session)
    {
        const string secretKey = "THISISMYSECURETOKEN";
        var client = _httpClientFactory.CreateClient();

        WriteToFile($"🔄 Iniciando geração de QR Code para sessão: {session}");

        try
        {
            // 1. Gerar token
            var tokenUrl = $"https://wppconnect-production-1747.up.railway.app/api/{session}/{secretKey}/generate-token";
            var tokenResponse = await client.PostAsync(tokenUrl, null);

            if (!tokenResponse.IsSuccessStatusCode)
            {
                var error = await tokenResponse.Content.ReadAsStringAsync();
                WriteToFile($"❌ Erro ao gerar token. Status: {tokenResponse.StatusCode}, Body: {error}");
                return StatusCode((int)tokenResponse.StatusCode, error);
            }

            var tokenJson = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();
            if (!tokenJson.TryGetProperty("token", out var tokenElement))
            {
                WriteToFile("❌ Token não retornado pela API.");
                return BadRequest("Token não retornado pela API.");
            }

            var token = tokenElement.GetString();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            WriteToFile("✅ Token gerado com sucesso.");

            // 2. Iniciar sessão e esperar QR Code
            var startUrl = $"https://wppconnect-production-1747.up.railway.app/api/{session}/start-session";
            var content = JsonContent.Create(new
            {
                webhook = "https://0e9d2321470d.ngrok-free.app/webhook",
                waitQrCode = true
            });

            var startResponse = await client.PostAsync(startUrl, content);
            if (!startResponse.IsSuccessStatusCode)
            {
                var error = await startResponse.Content.ReadAsStringAsync();
                WriteToFile($"❌ Erro ao iniciar sessão. Status: {startResponse.StatusCode}, Body: {error}");
                return StatusCode((int)startResponse.StatusCode, error);
            }

            var startJson = await startResponse.Content.ReadFromJsonAsync<JsonElement>();
            if (!startJson.TryGetProperty("qrcode", out var qrCodeBase64))
            {
                WriteToFile("❌ QR Code não retornado.");
                return BadRequest("QR Code não retornado.");
            }

            var qrString = qrCodeBase64.GetString();

            WriteToFile($"📨 QR bruto recebido: {qrString}");

            if (string.IsNullOrWhiteSpace(qrString))
            {
                WriteToFile("❌ QR Code retornado está vazio ou nulo.");
                return BadRequest("QR Code vazio ou nulo.");
            }

            // Permitir quando vem direto em base64
            string base64Data;
            if (qrString.StartsWith("data:image"))
            {
                base64Data = qrString.Substring(qrString.IndexOf(",") + 1);
            }
            else if (qrString.Length > 100) // assume que é base64 válido
            {
                base64Data = qrString;
            }
            else
            {
                WriteToFile("❌ QR Code retornado inválido ou em formato não suportado.");
                return BadRequest("QR Code inválido ou em formato não suportado.");
            }

            WriteToFile("📦 QR Code gerado com sucesso.");

            // 3. Esperar e checar status da sessão
            await Task.Delay(6000);

            var statusUrl = $"https://wppconnect-production-1747.up.railway.app/api/{session}/status-session";
            var statusResponse = await client.GetAsync(statusUrl);
            var statusJson = await statusResponse.Content.ReadFromJsonAsync<JsonElement>();

            if (statusJson.TryGetProperty("status", out var statusProp))
            {
                var status = statusProp.GetString();
                WriteToFile($"📡 Status da sessão após 6s: {status}");

                if (status != "CONNECTED")
                    WriteToFile($"⚠️ Sessão ainda não conectada. Status atual: {status}");
                else
                    WriteToFile("✅ Sessão conectada com sucesso!");
            }

            // 4. Retorna QR Code
            var bytes = Convert.FromBase64String(base64Data);
            return File(bytes, "image/png");
        }
        catch (Exception ex)
        {
            WriteToFile($"❌ Erro inesperado: {ex.Message} | Stack: {ex.StackTrace}");
            return StatusCode(500, "Erro interno ao buscar QR Code.");
        }
    }

    private void WriteToFile(string message)
    {
        var logPath = @"C:\logs\wppconnect.log";
        var logDir = Path.GetDirectoryName(logPath);

        if (!Directory.Exists(logDir))
        {
            Directory.CreateDirectory(logDir!);
        }

        var fullMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
        System.IO.File.AppendAllText(logPath, fullMessage);
    }
}
