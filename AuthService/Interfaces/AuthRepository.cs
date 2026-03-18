using AuthService.Models;
using Dapper;
using Npgsql;

namespace AuthService.Interfaces;

public class AuthRepository : IAuthRepository
{
    private readonly string _connectionString;

    public AuthRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private NpgsqlConnection CreateConnection() =>
        new NpgsqlConnection(_connectionString);

    public async Task<int> RegisterUserAsync(User user)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "usp_RegisterUser",
            new
            {
                p_FullName = user.FullName,           
                p_Email = user.Email,                 
                p_PasswordHash = user.PasswordHash,   
                p_Role = user.Role                    
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "usp_GetUserByEmail",
            new { p_Email = email }, 
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "usp_GetUserById",
            new { p_UserId = userId }, 
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}