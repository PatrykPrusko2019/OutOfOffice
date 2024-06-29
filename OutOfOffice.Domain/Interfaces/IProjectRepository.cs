
using OutOfOffice.Domain.Entities;

namespace OutOfOffice.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task Create(Project project);
        Task<IEnumerable<Project>> GetAll();
        Task<Project> GetProjectById(int id);
        Task Commit();
        Task Delete(Project project);
    }
}
