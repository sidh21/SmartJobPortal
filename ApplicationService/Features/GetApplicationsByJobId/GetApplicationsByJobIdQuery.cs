using ApplicationService.Models;
using MediatR;
using Shared.DTOs;

namespace ApplicationService.Features.GetApplicationsByJobId;

public record GetApplicationsByJobIdQuery(int JobId)
    : IRequest<ApiResponse<IEnumerable<Application>>>;