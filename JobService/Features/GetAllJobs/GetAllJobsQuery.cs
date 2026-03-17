using JobService.Models;
using MediatR;
using Shared.DTOs;

namespace JobService.Features.GetAllJobs;

public record GetAllJobsQuery() : IRequest<ApiResponse<IEnumerable<Job>>>;