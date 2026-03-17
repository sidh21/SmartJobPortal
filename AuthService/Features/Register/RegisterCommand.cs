using AuthService.DTOs;
using MediatR;
using Shared.DTOs;

namespace AuthService.Features.Register;

public record RegisterCommand(
    string FullName,
    string Email,
    string Password,
    string Role
) : IRequest<ApiResponse<AuthResponseDto>>;