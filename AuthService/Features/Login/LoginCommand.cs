using AuthService.DTOs;
using MediatR;
using Shared.DTOs;

namespace AuthService.Features.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<ApiResponse<AuthResponseDto>>;