using JobService.Models;

namespace JobService.Interfaces;

public interface IJobRepository
{
    Task<int> CreateJobAsync(Job job);
    Task<IEnumerable<Job>> GetAllJobsAsync();
    Task<Job?> GetJobByIdAsync(int jobId);
    Task UpdateJobAsync(Job job);
    Task DeleteJobAsync(int jobId);
}