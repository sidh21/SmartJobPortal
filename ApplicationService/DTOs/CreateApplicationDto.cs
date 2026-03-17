namespace ApplicationService.DTOs;

public class CreateApplicationDto
{
    public int JobId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string CandidateEmail { get; set; } = string.Empty;
    public string ResumeText { get; set; } = string.Empty;
    public string? CoverLetter { get; set; }
}