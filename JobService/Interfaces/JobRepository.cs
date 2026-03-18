using Dapper;
using JobService.Models;
using Npgsql;
using System.Data;

namespace JobService.Interfaces;

public class JobRepository : IJobRepository
{
    private readonly string _connectionString;

    public JobRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private NpgsqlConnection CreateConnection() =>
        new NpgsqlConnection(_connectionString);

    public async Task<int> CreateJobAsync(Job job)
    {
        using var conn = CreateConnection();
        var result = await conn.ExecuteScalarAsync<int>(
            "CALL \"usp_CreateJob\"(@p_Title, @p_Company, @p_Description, @p_Location, @p_Salary, @p_JobType, null)",
            new
            {
                p_Title = job.Title,
                p_Company = job.Company,
                p_Description = job.Description,
                p_Location = job.Location,
                p_Salary = job.Salary,
                p_JobType = job.JobType
            }
        );
        return result;
    }

    public async Task<IEnumerable<Job>> GetAllJobsAsync()
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<Job>(
            "SELECT * FROM \"usp_GetAllJobs\"()",  // Call function with SELECT
            commandType: System.Data.CommandType.Text
        );
    }

    public async Task<Job?> GetJobByIdAsync(int jobId)
    {
        using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Job>(
            "SELECT * FROM \"usp_GetJobById\"(@p_JobId)",  // Call function with SELECT
            new { p_JobId = jobId },
            commandType: System.Data.CommandType.Text
        );
    }

    public async Task UpdateJobAsync(Job job)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "CALL \"usp_UpdateJob\"(@p_JobId, @p_Title, @p_Company, @p_Description, @p_Location, @p_Salary, @p_JobType)",
            new
            {
                p_JobId = job.JobId,
                p_Title = job.Title,
                p_Company = job.Company,
                p_Description = job.Description,
                p_Location = job.Location,
                p_Salary = job.Salary,
                p_JobType = job.JobType
            }
        );
    }

    public async Task DeleteJobAsync(int jobId)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "CALL \"usp_DeleteJob\"(@p_JobId)",
            new { p_JobId = jobId }
        );
    }
}