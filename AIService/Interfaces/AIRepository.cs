using AIService.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AIService.Interfaces;

public class AIRepository : IAIRepository
{
    private readonly string _connectionString;

    public AIRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private SqlConnection CreateConnection() =>
        new SqlConnection(_connectionString);

    public async Task<int> SaveAIResultAsync(AIResult result)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "usp_SaveAIResult",
            new
            {
                result.ApplicationId,
                result.JobId,
                result.MatchScore,
                result.Strengths,
                result.Gaps,
                result.Recommendation
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<AIResult?> GetAIResultByApplicationIdAsync(int applicationId)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<AIResult>(
            "usp_GetAIResultByApplicationId",
            new { ApplicationId = applicationId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}