using AuthService.Models;
using Dapper;
using Npgsql;
using System.Data;

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

        var p_UserId = new NpgsqlParameter("p_UserId", NpgsqlTypes.NpgsqlDbType.Integer)
        {
            Direction = ParameterDirection.Output
        };

        await conn.ExecuteAsync(
            "CALL \"usp_RegisterUser\"(@p_FullName, @p_Email, @p_PasswordHash, @p_Role, @p_UserId)",
            new NpgsqlParameter[]
            {
                new NpgsqlParameter("p_FullName", user.FullName),
                new NpgsqlParameter("p_Email", user.Email),
                new NpgsqlParameter("p_PasswordHash", user.PasswordHash),
                new NpgsqlParameter("p_Role", user.Role),
                p_UserId
            }
        );
        return (int)p_UserId.Value;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using var conn = CreateConnection();
        var sql = "SELECT * FROM \"Users\" WHERE \"Email\" = @Email";
        return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        using var conn = CreateConnection();
        var sql = "SELECT * FROM \"Users\" WHERE \"UserId\" = @UserId";
        return await conn.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
    }
}