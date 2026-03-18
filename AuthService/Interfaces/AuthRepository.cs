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

        // Use raw SQL with CALL statement instead of StoredProcedure type
        var sql = "CALL \"usp_RegisterUser\"(@p_FullName, @p_Email, @p_PasswordHash, @p_Role, null)";

        var parameters = new
        {
            p_FullName = user.FullName,
            p_Email = user.Email,
            p_PasswordHash = user.PasswordHash,
            p_Role = user.Role
        };

        await conn.ExecuteAsync(sql, parameters);

        // Get the last inserted ID
        var userId = await conn.ExecuteScalarAsync<int>("SELECT LASTVAL();");
        return userId;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using var conn = CreateConnection();
        var sql = "SELECT * FROM \"Users\" WHERE \"Email\" = @p_Email";
        return await conn.QueryFirstOrDefaultAsync<User>(sql, new { p_Email = email });
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        using var conn = CreateConnection();
        var sql = "SELECT * FROM \"Users\" WHERE \"UserId\" = @p_UserId";
        return await conn.QueryFirstOrDefaultAsync<User>(sql, new { p_UserId = userId });
    }
}