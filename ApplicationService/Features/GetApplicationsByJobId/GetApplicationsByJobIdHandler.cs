using ApplicationService.Interfaces;
using ApplicationService.Models;
using MediatR;
using Shared.DTOs;

namespace ApplicationService.Features.GetApplicationsByJobId;

public class GetApplicationsByJobIdHandler
    : IRequestHandler<GetApplicationsByJobIdQuery, ApiResponse<IEnumerable<Application>>>
{
    private readonly IApplicationRepository _repository;

    public GetApplicationsByJobIdHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<Application>>> Handle(
        GetApplicationsByJobIdQuery request, CancellationToken cancellationToken)
    {
        var applications = await _repository.GetApplicationsByJobIdAsync(request.JobId);
        return ApiResponse<IEnumerable<Application>>.Ok(applications);
    }
}