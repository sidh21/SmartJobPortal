using MediatR;
using Shared.DTOs;

namespace AIService.Features.GenerateJD;

public record GenerateJDCommand(
    string JobTitle,
    string Company,
    string Location,
    string JobType
) : IRequest<ApiResponse<string>>;