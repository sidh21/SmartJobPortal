using Dapper;
using JobService.Models;
using Microsoft.Data.SqlClient;

namespace JobService.Interfaces;

public class JobRepository : IJobRepository
{
    private readonly string _connectionString;

    public JobRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private SqlConnection CreateConnection() =>
        new SqlConnection(_connectionString);

    public async Task<int> CreateJobAsync(Job job)
    {
        using var conn = CreateConnection();
        var result = await conn.ExecuteScalarAsync<int>(
            "usp_CreateJob",
            new
            {
                job.Title,
                job.Company,
                job.Description,
                job.Location,
                job.Salary,
                job.JobType
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
        return result;
    }

    public async Task<IEnumerable<Job>> GetAllJobsAsync()
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Job>(
            "usp_GetAllJobs",
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<Job?> GetJobByIdAsync(int jobId)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Job>(
            "usp_GetJobById",
            new { JobId = jobId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task UpdateJobAsync(Job job)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "usp_UpdateJob",
            new
            {
                job.JobId,
                job.Title,
                job.Company,
                job.Description,
                job.Location,
                job.Salary,
                job.JobType
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task DeleteJobAsync(int jobId)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "usp_DeleteJob",
            new { JobId = jobId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}