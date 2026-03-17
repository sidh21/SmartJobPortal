using JobService.Interfaces;
using JobService.Models;
using MediatR;
using Shared.DTOs;
using Shared.Helpers;

namespace JobService.Features.GetJobById;

public class GetJobByIdHandler : IRequestHandler<GetJobByIdQuery, ApiResponse<Job>>
{
    private readonly IJobRepository _repository;

    public GetJobByIdHandler(IJobRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<Job>> Handle(
        GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await _repository.GetJobByIdAsync(request.JobId);

        if (job is null)
            return ApiResponse<Job>.Fail(ResponseHelper.NotFound("Job"));

        return ApiResponse<Job>.Ok(job);
    }
}