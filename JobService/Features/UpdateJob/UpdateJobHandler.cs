using JobService.Interfaces;
using JobService.Models;
using MediatR;
using Shared.DTOs;
using Shared.Helpers;

namespace JobService.Features.UpdateJob;

public class UpdateJobHandler : IRequestHandler<UpdateJobCommand, ApiResponse<bool>>
{
    private readonly IJobRepository _repository;

    public UpdateJobHandler(IJobRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> Handle(
        UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetJobByIdAsync(request.JobId);

        if (existing is null)
            return ApiResponse<bool>.Fail(ResponseHelper.NotFound("Job"));

        var job = new Job
        {
            JobId = request.JobId,
            Title = request.Title,
            Company = request.Company,
            Description = request.Description,
            Location = request.Location,
            Salary = request.Salary,
            JobType = request.JobType
        };

        await _repository.UpdateJobAsync(job);
        return ApiResponse<bool>.Ok(true, ResponseHelper.Updated("Job"));
    }
}