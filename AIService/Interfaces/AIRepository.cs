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

        return await conn.ExecuteScalarAsync<int>(
            @"SELECT ""usp_SaveAIResult""(
                @p_ApplicationId,
                @p_JobId,
                @p_MatchScore,
                @p_Strengths,
                @p_Gaps,
                @p_Recommendation
            )",
            new
            {
                p_ApplicationId = result.ApplicationId,
                p_JobId = result.JobId,
                p_MatchScore = result.MatchScore,
                p_Strengths = result.Strengths,
                p_Gaps = result.Gaps,
                p_Recommendation = result.Recommendation
            }
            // ✅ NO commandType here — raw SQL, not StoredProcedure
        );
    }

    public async Task<AIResult?> GetAIResultByApplicationIdAsync(int applicationId)
    {
        using var conn = CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<AIResult>(
            @"SELECT * FROM ""usp_GetAIResultByApplicationId""(@p_ApplicationId)",
            new { p_ApplicationId = applicationId }
            // ✅ NO commandType here — raw SQL, not StoredProcedure
        );
    }
}