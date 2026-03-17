namespace ApplicationService.Models;

public class Application
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public string ResumeText { get; set; } = string.Empty;
    public string? CoverLetter { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime AppliedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}