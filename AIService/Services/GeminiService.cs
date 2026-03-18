using AIService.Interfaces;
using System.Text;
using System.Text.Json;

namespace AIService.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly bool _useGroq;

    public GeminiService(HttpClient http, IConfiguration config)
    {
        _http = http;

        // Check which API to use
        _apiKey = config["Groq:ApiKey"] ?? config["Gemini:ApiKey"]
            ?? throw new InvalidOperationException("No AI API Key configured");

        _useGroq = !string.IsNullOrEmpty(config["Groq:ApiKey"]);
    }

    public async Task<string> GenerateAsync(
        string prompt, CancellationToken ct = default)
    {
        if (_useGroq)
        {
            return await GenerateWithGroqAsync(prompt, ct);
        }
        else
        {
            return await GenerateWithGeminiAsync(prompt, ct);
        }
    }

    private async Task<string> GenerateWithGroqAsync(
        string prompt, CancellationToken ct)
    {
        var url = "https://api.groq.com/openai/v1/chat/completions";

        var body = new
        {
            model = "llama-3.1-70b-versatile", // or "mixtral-8x7b-32768"
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            temperature = 0.7,
            max_tokens = 2048
        };

        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

        var response = await _http.PostAsync(url, content, ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            throw new Exception($"Groq API Error: {response.StatusCode} - {error}");
        }

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var document = JsonDocument.Parse(responseJson);

        return document
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }

    private async Task<string> GenerateWithGeminiAsync(
        string prompt, CancellationToken ct)
    {
        var model = "gemini-1.5-flash";
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/" +
                  $"{model}:generateContent?key={_apiKey}";

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[] { new { text = prompt } }
                }
            }
        };

        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync(url, content, ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            throw new Exception($"Gemini API Error: {response.StatusCode} - {error}");
        }

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
}
