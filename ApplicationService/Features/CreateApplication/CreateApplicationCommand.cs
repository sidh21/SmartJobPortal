using MediatR;
using Shared.DTOs;

namespace ApplicationService.Features.CreateApplication;

public record CreateApplicationCommand(
    int JobId,
    string CandidateName,
    string CandidateEmail,
    string ResumeText,
    string? CoverLetter
) : IRequest<ApiResponse<int>>;