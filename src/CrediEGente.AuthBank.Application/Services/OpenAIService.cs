using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CapitalNerd.Laranjinhai.Application.Interfaces;
using CapitalNerd.Laranjinhai.Application.Settings;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Options;

namespace CapitalNerd.Laranjinhai.Application.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAISettings _settings;

        public OpenAIService(HttpClient httpClient, IOptions<OpenAISettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;

            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
        }

        public async Task<string> ObterRespostaAsync(string pergunta, CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "Você é um atendente virtual educado e objetivo." },
                    new { role = "user", content = pergunta }
                }
            };

            var response = await _httpClient.PostAsJsonAsync("chat/completions", request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return "Desculpe, não consegui gerar uma resposta no momento.";

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
            return document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
    }
}
