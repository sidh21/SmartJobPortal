using AIService.Interfaces;
using System.Text;
using System.Text.Json;

namespace AIService.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _provider;

    public GeminiService(HttpClient http, IConfiguration config)
    {
        _http = http;

        // Try Groq first (more reliable), then Gemini as fallback
        var groqKey = config["Groq:ApiKey"];
        var geminiKey = config["Gemini:ApiKey"];

        if (!string.IsNullOrEmpty(groqKey))
        {
            _apiKey = groqKey;
            _provider = "Groq";
            Console.WriteLine("✅ Using Groq AI API");
        }
        else if (!string.IsNullOrEmpty(geminiKey))
        {
            _apiKey = geminiKey;
            _provider = "Gemini";
            Console.WriteLine("✅ Using Gemini AI API");
        }
        else
        {
            throw new InvalidOperationException(
                "No AI API Key configured. Set either 'Groq:ApiKey' or 'Gemini:ApiKey' in environment variables.");
        }
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
    {
        try
        {
            if (_provider == "Groq")
            {
                return await GenerateWithGroqAsync(prompt, ct);
            }
            else
            {
                return await GenerateWithGeminiAsync(prompt, ct);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ AI Generation Error: {ex.Message}");
            throw;
        }
    }

    private async Task<string> GenerateWithGroqAsync(string prompt, CancellationToken ct)
    {
        var models = new[]
        {
            "llama-3.3-70b-versatile",
            "llama-3.1-70b-versatile",
            "mixtral-8x7b-32768"
        };

        Exception? lastException = null;

        foreach (var model in models)
        {
            try
            {
                var url = "https://api.groq.com/openai/v1/chat/completions";

                var body = new
                {
                    model = model,
                    messages = new[]
                    {
                        new
                        {
                            role = "system",
                            content = "You are a helpful AI assistant specialized in HR and recruitment."
                        },
                        new
                        {
                            role = "user",
                            content = prompt
                        }
                    },
                    temperature = 0.7,
                    max_tokens = 2048,
                    top_p = 0.9
                };

                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                var response = await _http.PostAsync(url, content, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"⚠️ Groq Error ({model}): {response.StatusCode} - {error}");
                    lastException = new Exception($"Groq {model}: {response.StatusCode}");

                    // Rate limit - try next model
                    if ((int)response.StatusCode == 429)
                    {
                        await Task.Delay(2000, ct);
                        continue;
                    }
                    continue;
                }

                var responseJson = await response.Content.ReadAsStringAsync(ct);
                var document = JsonDocument.Parse(responseJson);

                var text = document
                    .RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new Exception("Empty response from Groq");
                }

                Console.WriteLine($"✅ Groq Success with model: {model}");
                return text;
            }
            catch (Exception ex)
            {
                lastException = ex;
                Console.WriteLine($"⚠️ Error with Groq model {model}: {ex.Message}");
                await Task.Delay(1000, ct);
                continue;
            }
        }

        throw new Exception(
            $"All Groq models failed. Last error: {lastException?.Message}. " +
            $"Please check your API key at https://console.groq.com/keys");
    }

    private async Task<string> GenerateWithGeminiAsync(string prompt, CancellationToken ct)
    {
        var models = new[]
        {
            "gemini-1.5-flash-latest",
            "gemini-1.5-flash",
            "gemini-1.5-pro-latest"
        };

        Exception? lastException = null;

        foreach (var model in models)
        {
            try
            {
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={_apiKey}";

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
                        topP = 0.9,
                        topK = 40
                    },
                    safetySettings = new[]
                    {
                        new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_NONE" },
                        new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_NONE" },
                        new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_NONE" },
                        new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_NONE" }
                    }
                };

                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _http.DefaultRequestHeaders.Clear();

                var response = await _http.PostAsync(url, content, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine($"⚠️ Gemini Error ({model}): {response.StatusCode} - {error}");
                    lastException = new Exception($"Gemini {model}: {response.StatusCode}");

                    // Rate limit - try next model
                    if ((int)response.StatusCode == 429)
                    {
                        await Task.Delay(2000, ct);
                        continue;
                    }
                    continue;
                }

                var responseJson = await response.Content.ReadAsStringAsync(ct);
                var document = JsonDocument.Parse(responseJson);

                var text = document
                    .RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new Exception("Empty response from Gemini");
                }

                Console.WriteLine($"✅ Gemini Success with model: {model}");
                return text;
            }
            catch (Exception ex)
            {
                lastException = ex;
                Console.WriteLine($"⚠️ Error with Gemini model {model}: {ex.Message}");
                await Task.Delay(1000, ct);
                continue;
            }
        }

        throw new Exception(
            $"All Gemini models failed. Last error: {lastException?.Message}. " +
            $"Please check your API key at https://aistudio.google.com/app/apikey");
    }
}