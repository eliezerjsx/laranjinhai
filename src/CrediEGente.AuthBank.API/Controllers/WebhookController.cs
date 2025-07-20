using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CapitalNerd.Laranjinhai.Application.Interfaces;
using System.Text.Json;

namespace CapitalNerd.Laranjinhai.API.Controllers;

[ApiController]
[Route("webhook")]
public class WebhookController : ControllerBase
{
    private readonly ILogger<WebhookController> _logger;
    private readonly IMensagemService _messageService;

    public WebhookController(
        ILogger<WebhookController> logger,
        IMensagemService messageService)
    {
        _logger = logger;
        _messageService = messageService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] JsonElement body)
    {
        _logger.LogInformation("📩 Webhook recebido do WhatsConnect: {Body}", body.ToString());

        try
        {
            await _messageService.SalvarMensagemRecebidaAsync(body);
            return Ok(new { status = "mensagem processada com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao processar e salvar mensagem recebida.");
            return StatusCode(500, "Erro ao processar mensagem.");
        }
    }
}
