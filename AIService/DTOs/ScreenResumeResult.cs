namespace AIService.DTOs;

public class ScreenResumeResult
{
    public int MatchScore { get; set; }
    public List<string> Strengths { get; set; } = new();
    public List<string> Gaps { get; set; } = new();
    public string Recommendation { get; set; } = string.Empty;
}