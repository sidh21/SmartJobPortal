using MediatR;
using Shared.DTOs;

namespace ApplicationService.Features.UpdateApplicationStatus;

public record UpdateApplicationStatusCommand(
    int ApplicationId,
    string Status
) : IRequest<ApiResponse<bool>>;