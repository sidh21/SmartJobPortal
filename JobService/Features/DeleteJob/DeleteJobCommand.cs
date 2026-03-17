using MediatR;
using Shared.DTOs;

namespace JobService.Features.DeleteJob;

public record DeleteJobCommand(int JobId) : IRequest<ApiResponse<bool>>;