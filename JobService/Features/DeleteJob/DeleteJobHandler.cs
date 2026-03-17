using JobService.Interfaces;
using MediatR;
using Shared.DTOs;
using Shared.Helpers;

namespace JobService.Features.DeleteJob;

public class DeleteJobHandler : IRequestHandler<DeleteJobCommand, ApiResponse<bool>>
{
    private readonly IJobRepository _repository;

    public DeleteJobHandler(IJobRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> Handle(
        DeleteJobCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetJobByIdAsync(request.JobId);

        if (existing is null)
            return ApiResponse<bool>.Fail(ResponseHelper.NotFound("Job"));

        await _repository.DeleteJobAsync(request.JobId);
        return ApiResponse<bool>.Ok(true, ResponseHelper.Deleted("Job"));
    }
}