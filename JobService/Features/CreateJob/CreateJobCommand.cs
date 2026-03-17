using MediatR;
using Shared.DTOs;

namespace JobService.Features.CreateJob;

public record CreateJobCommand(
    string Title,
    string Company,
    string Description,
    string Location,
    decimal? Salary,
    string JobType
) : IRequest<ApiResponse<int>>;