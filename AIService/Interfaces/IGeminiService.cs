namespace AIService.Interfaces;

public interface IGeminiService
{
    Task<string> GenerateAsync(string prompt, CancellationToken ct = default);
}