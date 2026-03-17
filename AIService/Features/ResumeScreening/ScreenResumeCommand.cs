using AIService.DTOs;
using MediatR;
using Shared.DTOs;

namespace AIService.Features.ResumeScreening;

public record ScreenResumeCommand(
    int ApplicationId,
    int JobId,
    string ResumeText,
    string JobDescription
) : IRequest<ApiResponse<ScreenResumeResult>>;  