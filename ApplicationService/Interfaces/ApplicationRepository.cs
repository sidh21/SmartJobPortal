using ApplicationService.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ApplicationService.Interfaces;

public class ApplicationRepository : IApplicationRepository
{
    private readonly string _connectionString;

    public ApplicationRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private SqlConnection CreateConnection() =>
        new SqlConnection(_connectionString);

    public async Task<int> CreateApplicationAsync(Application application)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "usp_CreateApplication",
            new
            {
                application.JobId,
                application.CandidateName,
                application.CandidateEmail,
                application.ResumeText,
                application.CoverLetter
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<Application>> GetApplicationsByJobIdAsync(int jobId)
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Application>(
            "usp_GetApplicationsByJobId",
            new { JobId = jobId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<Application?> GetApplicationByIdAsync(int applicationId)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Application>(
            "usp_GetApplicationById",
            new { ApplicationId = applicationId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task UpdateApplicationStatusAsync(int applicationId, string status)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "usp_UpdateApplicationStatus",
            new { ApplicationId = applicationId, Status = status },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}