using JobService.Interfaces;
using JobService.Models;
using MediatR;
using Shared.DTOs;
using Shared.Helpers;

namespace JobService.Features.CreateJob;

public class CreateJobHandler : IRequestHandler<CreateJobCommand, ApiResponse<int>>
{
    private readonly IJobRepository _repository;

    public CreateJobHandler(IJobRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<int>> Handle(
        CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = new Job
        {
            Title = request.Title,
            Company = request.Company,
            Description = request.Description,
            Location = request.Location,
            Salary = request.Salary,
            JobType = request.JobType
        };

        var jobId = await _repository.CreateJobAsync(job);
        return ApiResponse<int>.Ok(jobId, ResponseHelper.Created("Job"));
    }
}