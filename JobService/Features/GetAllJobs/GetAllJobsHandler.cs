using JobService.Interfaces;
using JobService.Models;
using MediatR;
using Shared.DTOs;

namespace JobService.Features.GetAllJobs;

public class GetAllJobsHandler
    : IRequestHandler<GetAllJobsQuery, ApiResponse<IEnumerable<Job>>>
{
    private readonly IJobRepository _repository;

    public GetAllJobsHandler(IJobRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<Job>>> Handle(
        GetAllJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _repository.GetAllJobsAsync();
        return ApiResponse<IEnumerable<Job>>.Ok(jobs);
    }
}