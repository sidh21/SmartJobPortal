using ApplicationService.Models;
using Dapper;
using Npgsql;

namespace ApplicationService.Interfaces;

public class ApplicationRepository : IApplicationRepository
{
    private readonly string _connectionString;

    public ApplicationRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private NpgsqlConnection CreateConnection() =>
        new NpgsqlConnection(_connectionString);

    public async Task<int> CreateApplicationAsync(Application application)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            "usp_CreateApplication",
            new
            {
                p_JobId = application.JobId,               
                p_CandidateName = application.CandidateName,
                p_CandidateEmail = application.CandidateEmail,
                p_ResumeText = application.ResumeText,       
                p_CoverLetter = application.CoverLetter   
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<Application>> GetApplicationsByJobIdAsync(int jobId)
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Application>(
            "usp_GetApplicationsByJobId",
            new { p_JobId = jobId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<Application?> GetApplicationByIdAsync(int applicationId)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Application>(
            "usp_GetApplicationById",
            new { p_ApplicationId = applicationId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task UpdateApplicationStatusAsync(int applicationId, string status)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "usp_UpdateApplicationStatus",
            new
            {
                p_ApplicationId = applicationId,  
                p_Status = status                 
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}