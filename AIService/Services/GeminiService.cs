using AIService.Interfaces;
using System.Text;
using System.Text.Json;

namespace AIService.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    // Models to try in order — if one fails move to next
    private readonly string[] _models = new[]
    {
        "gemini-2.0-flash-lite",
        "gemini-2.0-flash",
        "gemini-2.5-flash",
        "gemini-2.0-flash-001"
    };

    public GeminiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["Gemini:ApiKey"]!;
    }

    public async Task<string> GenerateAsync(
        string prompt, CancellationToken ct = default)
    {
        Exception? lastException = null;

        foreach (var model in _models)
        {
            try
            {
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/" +
                          $"{model}:generateContent?key={_apiKey}";

                var body = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(
                    json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content, ct);

                // If rate limited try next model
                if ((int)response.StatusCode == 429)
                {
                    lastException = new Exception(
                        $"Model {model} rate limited, trying next...");
                    continue;
                }

                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content
                    .ReadAsStringAsync(ct);
                var document = JsonDocument.Parse(responseJson);

                return document
                    .RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString()!;
            }
            catch (Exception ex) when (ex.Message.Contains("429")
                                    || ex.Message.Contains("rate limit")
                                    || ex.Message.Contains("Rate limit"))
            {
                lastException = ex;
                // Wait 2 seconds before trying next model
                await Task.Delay(2000, ct);
                continue;
            }
        }

        throw new Exception(
            $"All Gemini models are rate limited. Please wait a minute and try again. " +
            $"Last error: {lastException?.Message}");
    }
}