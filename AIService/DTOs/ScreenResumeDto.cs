namespace AIService.DTOs;

public class ScreenResumeDto
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }
    public string ResumeText { get; set; } = string.Empty;
    public string JobDescription { get; set; } = string.Empty;
}