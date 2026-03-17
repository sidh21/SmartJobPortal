using MediatR;
using Shared.DTOs;

namespace JobService.Features.UpdateJob;

public record UpdateJobCommand(
    int JobId,
    string Title,
    string Company,
    string Description,
    string Location,
    decimal? Salary,
    string JobType
) : IRequest<ApiResponse<bool>>;