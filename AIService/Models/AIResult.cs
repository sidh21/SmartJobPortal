namespace AIService.Models;

public class AIResult
{
    public int ResultId { get; set; }
    public int ApplicationId { get; set; }
    public int JobId { get; set; }
    public int MatchScore { get; set; }
    public string? Strengths { get; set; }
    public string? Gaps { get; set; }
    public string? Recommendation { get; set; }
    public DateTime CreatedAt { get; set; }
}