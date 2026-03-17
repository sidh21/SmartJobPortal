using AuthService.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AuthService.Interfaces;

public class AuthRepository : IAuthRepository
{
    private readonly string _connectionString;

    public AuthRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private SqlConnection CreateConnection() =>
        new SqlConnection(_connectionString);

    public async Task<int> RegisterUserAsync(User user)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "usp_RegisterUser",
            new
            {
                user.FullName,
                user.Email,
                user.PasswordHash,
                user.Role
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "usp_GetUserByEmail",
            new { Email = email },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "usp_GetUserById",
            new { UserId = userId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}