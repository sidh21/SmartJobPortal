using ApplicationService.Models;

namespace ApplicationService.Interfaces;

public interface IApplicationRepository
{
    Task<int> CreateApplicationAsync(Application application);
    Task<IEnumerable<Application>> GetApplicationsByJobIdAsync(int jobId);
    Task<Application?> GetApplicationByIdAsync(int applicationId);
    Task UpdateApplicationStatusAsync(int applicationId, string status);
}