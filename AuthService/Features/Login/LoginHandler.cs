using AuthService.DTOs;
using AuthService.Interfaces;
using MediatR;
using Shared.DTOs;

namespace AuthService.Features.Login;

public class LoginHandler
    : IRequestHandler<LoginCommand, ApiResponse<AuthResponseDto>>
{
    private readonly IAuthRepository _repository;
    private readonly IJwtService _jwtService;

    public LoginHandler(IAuthRepository repository, IJwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<AuthResponseDto>> Handle(
        LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByEmailAsync(request.Email);

        if (user is null)
            return ApiResponse<AuthResponseDto>.Fail("Invalid email or password.");

        var isPasswordValid = BCrypt.Net.BCrypt
            .Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
            return ApiResponse<AuthResponseDto>.Fail("Invalid email or password.");

        var token = _jwtService.GenerateToken(user);

        return ApiResponse<AuthResponseDto>.Ok(new AuthResponseDto
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        }, "Login successful!");
    }
}