using AuthService.DTOs;
using AuthService.Interfaces;
using AuthService.Models;
using MediatR;
using Shared.DTOs;

namespace AuthService.Features.Register;

public class RegisterHandler
    : IRequestHandler<RegisterCommand, ApiResponse<AuthResponseDto>>
{
    private readonly IAuthRepository _repository;
    private readonly IJwtService _jwtService;

    public RegisterHandler(IAuthRepository repository, IJwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<AuthResponseDto>> Handle(
        RegisterCommand request, CancellationToken cancellationToken)
    {
        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = request.Role
        };

        var userId = await _repository.RegisterUserAsync(user);
        user.UserId = userId;

        var token = _jwtService.GenerateToken(user);

        return ApiResponse<AuthResponseDto>.Ok(new AuthResponseDto
        {
            UserId = userId,
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        }, "Registration successful!");
    }
}