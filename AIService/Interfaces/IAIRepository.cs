using AIService.Models;

namespace AIService.Interfaces;

public interface IAIRepository
{
    Task<int> SaveAIResultAsync(AIResult result);
    Task<AIResult?> GetAIResultByApplicationIdAsync(int applicationId);
}