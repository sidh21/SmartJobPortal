using ApplicationService.Interfaces;
using ApplicationService.Models;
using MediatR;
using Shared.DTOs;
using Shared.Helpers;

namespace ApplicationService.Features.CreateApplication;

public class CreateApplicationHandler
    : IRequestHandler<CreateApplicationCommand, ApiResponse<int>>
{
    private readonly IApplicationRepository _repository;

    public CreateApplicationHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<int>> Handle(
        CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = new Application
        {
            JobId = request.JobId,
            CandidateName = request.CandidateName,
            CandidateEmail = request.CandidateEmail,
            ResumeText = request.ResumeText,
            CoverLetter = request.CoverLetter
        };

        var id = await _repository.CreateApplicationAsync(application);
        return ApiResponse<int>.Ok(id, ResponseHelper.Created("Application"));
    }
}