using AIService.Interfaces;
using System.Text;
using System.Text.Json;

namespace AIService.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    // ✅ Updated model names (current as of 2024)
    private readonly string[] _models = new[]
    {
        "gemini-1.5-flash-latest",
        "gemini-1.5-flash",
        "gemini-pro"
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
                // ✅ Updated API endpoint (v1 instead of v1beta)
                var url = $"https://generativelanguage.googleapis.com/v1/models/" +
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
                        maxOutputTokens = 2048,
                        topP = 0.8,
                        topK = 10
                    }
                };

                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(
                    json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync(url, content, ct);

                // Log the full error for debugging
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"Gemini API Error ({model}): {response.StatusCode}");
                    Console.WriteLine($"Error Details: {errorContent}");
                    Console.WriteLine($"URL Attempted: {url}");

                    // If 404, model doesn't exist - try next
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        lastException = new Exception(
                            $"Model {model} not found (404)");
                        continue;
                    }

                    // If 403, API key issue
                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        lastException = new Exception(
                            $"API key invalid or restricted (403): {errorContent}");
                        continue;
                    }

                    // If rate limited, try next model
                    if ((int)response.StatusCode == 429)
                    {
                        lastException = new Exception($"Model {model} rate limited");
                        await Task.Delay(2000, ct);
                        continue;
                    }
                }

                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync(ct);
                Console.WriteLine($"Success with model: {model}");

                var document = JsonDocument.Parse(responseJson);

                var text = document
                    .RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? throw new Exception("Empty response from Gemini");
            }
            catch (HttpRequestException ex)
            {
                lastException = ex;
                Console.WriteLine($"HTTP Error with model {model}: {ex.Message}");
                await Task.Delay(1000, ct);
                continue;
            }
            catch (JsonException ex)
            {
                lastException = ex;
                Console.WriteLine($"JSON Parse Error with model {model}: {ex.Message}");
                await Task.Delay(1000, ct);
                continue;
            }
            catch (Exception ex)
            {
                lastException = ex;
                Console.WriteLine($"Error with model {model}: {ex.Message}");
                await Task.Delay(1000, ct);
                continue;
            }
        }

        throw new Exception(
            $"All Gemini models failed. Last error: {lastException?.Message}. " +
            $"Please verify your API key at https://aistudio.google.com/app/apikey " +
            $"and check the Render logs for detailed error messages.");
    }
}