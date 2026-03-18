using Dapper;
using JobService.Models;
using Npgsql; 
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
            "usp_CreateJob",
            new
            {
                p_Title = job.Title,             
                p_Company = job.Company,         
                p_Description = job.Description, 
                p_Location = job.Location,       
                p_Salary = job.Salary,           
                p_JobType = job.JobType          
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
            new { p_JobId = jobId }, 
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
                p_JobId = job.JobId,              
                p_Title = job.Title,              
                p_Company = job.Company,          
                p_Description = job.Description,  
                p_Location = job.Location,        
                p_Salary = job.Salary,            
                p_JobType = job.JobType           
            },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task DeleteJobAsync(int jobId)
    {
        using var conn = CreateConnection();
        await conn.ExecuteAsync(
            "usp_DeleteJob",
            new { p_JobId = jobId },  
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}