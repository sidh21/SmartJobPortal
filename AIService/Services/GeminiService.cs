using AIService.Interfaces;
using System.Text;
using System.Text.Json;

namespace AIService.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    // ✅ Use only free tier models
    private readonly string[] _models = new[]
    {
        "gemini-1.5-flash",
        "gemini-1.5-flash-8b"
    };

    public GeminiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["Gemini:ApiKey"]
            ?? throw new InvalidOperationException("Gemini API Key not configured");
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
                    },
                    generationConfig = new
                    {
                        temperature = 0.7,
                        maxOutputTokens = 2048
                    }
                };

                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(
                    json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content, ct);

                // Log the error for debugging
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"Gemini API Error ({model}): {response.StatusCode} - {errorContent}");

                    // If 403, try next model
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        lastException = new Exception(
                            $"Model {model} returned 403. Error: {errorContent}");
                        continue;
                    }

                    // If rate limited, try next model
                    if ((int)response.StatusCode == 429)
                    {
                        lastException = new Exception(
                            $"Model {model} rate limited");
                        await Task.Delay(2000, ct);
                        continue;
                    }
                }

                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync(ct);
                var document = JsonDocument.Parse(responseJson);

                return document
                    .RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString()!;
            }
            catch (Exception ex)
            {
                lastException = ex;
                Console.WriteLine($"Error with model {model}: {ex.Message}");

                // Wait before trying next model
                await Task.Delay(1000, ct);
                continue;
            }
        }

        throw new Exception(
            $"All Gemini models failed. Last error: {lastException?.Message}. " +
            $"Please check your API key at https://aistudio.google.com/app/apikey");
    }
}