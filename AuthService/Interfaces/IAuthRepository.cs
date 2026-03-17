using AuthService.Models;

namespace AuthService.Interfaces;

public interface IAuthRepository
{
    Task<int> RegisterUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(int userId);
}