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

        var parameters = new DynamicParameters();
        parameters.Add("p_FullName", user.FullName);
        parameters.Add("p_Email", user.Email);
        parameters.Add("p_PasswordHash", user.PasswordHash);
        parameters.Add("p_Role", user.Role);
        parameters.Add("p_UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(
            "CALL \"usp_RegisterUser\"(@p_FullName, @p_Email, @p_PasswordHash, @p_Role, @p_UserId)",
            parameters
        );

        return parameters.Get<int>("p_UserId");
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