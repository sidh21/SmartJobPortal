using AuthService.Models;

namespace AuthService.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}