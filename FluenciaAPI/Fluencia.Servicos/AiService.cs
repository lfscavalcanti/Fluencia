using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Fluencia.Servicos.Interfaces;

namespace Fluencia.Servicos
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AiService(IConfiguration config, HttpClient? httpClient = null)
        {
            _config = config;
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<string> GetAiResponseAsync(string prompt)
        {
            var url = _config["GitHubModels:ApiUrl"];
            var token = _config["GitHubModels:Token"];

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            _httpClient.DefaultRequestHeaders.UserAgent.Clear();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Itera360App");

            var content = new StringContent(
                JsonSerializer.Serialize(new { prompt = prompt, max_tokens = 600 }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return $"Erro: {response.StatusCode} - {error}";
            }

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}