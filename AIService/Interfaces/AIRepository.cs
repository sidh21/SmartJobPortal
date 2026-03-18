using AIService.Models;
using Dapper;
using Npgsql;

namespace AIService.Interfaces;

public class AIRepository : IAIRepository
{
    private readonly string _connectionString;

    public AIRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private NpgsqlConnection CreateConnection() =>
        new NpgsqlConnection(_connectionString);

    public async Task<int> SaveAIResultAsync(AIResult result)
    {
        using var conn = CreateConnection();

        // ✅ FIXED: Added quotes for PostgreSQL case-sensitive names
        return await conn.ExecuteScalarAsync<int>(
            "\"usp_SaveAIResult\"",
            new
            {
                p_ApplicationId = result.ApplicationId,
                p_JobId = result.JobId,
                p_MatchScore = result.MatchScore,
                p_Strengths = result.Strengths,
                p_Gaps = result.Gaps,
                p_Recommendation = result.Recommendation
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<AIResult?> GetAIResultByApplicationIdAsync(int applicationId)
    {
        using var conn = CreateConnection();

        // ✅ FIXED: Added quotes for PostgreSQL
        return await conn.QueryFirstOrDefaultAsync<AIResult>(
            "\"usp_GetAIResultByApplicationId\"",
            new { p_ApplicationId = applicationId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}