using JobService.Models;
using MediatR;
using Shared.DTOs;

namespace JobService.Features.GetJobById;

public record GetJobByIdQuery(int JobId) : IRequest<ApiResponse<Job>>;