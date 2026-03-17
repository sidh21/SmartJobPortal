using ApplicationService.Interfaces;
using MediatR;
using Shared.DTOs;
using Shared.Helpers;

namespace ApplicationService.Features.UpdateApplicationStatus;

public class UpdateApplicationStatusHandler
    : IRequestHandler<UpdateApplicationStatusCommand, ApiResponse<bool>>
{
    private readonly IApplicationRepository _repository;

    public UpdateApplicationStatusHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> Handle(
        UpdateApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetApplicationByIdAsync(request.ApplicationId);

        if (existing is null)
            return ApiResponse<bool>.Fail(ResponseHelper.NotFound("Application"));

        var validStatuses = new[] { "Pending", "Shortlisted", "Rejected" };
        if (!validStatuses.Contains(request.Status))
            return ApiResponse<bool>.Fail("Status must be Pending, Shortlisted or Rejected.");

        await _repository.UpdateApplicationStatusAsync(request.ApplicationId, request.Status);
        return ApiResponse<bool>.Ok(true, ResponseHelper.Updated("Application status"));
    }
}