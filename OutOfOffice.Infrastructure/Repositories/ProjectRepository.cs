
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Domain.Entities;
using OutOfOffice.Domain.Interfaces;
using OutOfOffice.Infrastructure.Persistence;

namespace OutOfOffice.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly OutOfOfficeDbContext _dbContext;

        public ProjectRepository(OutOfOfficeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        => await _dbContext.SaveChangesAsync();

        public async Task Create(Project project)
        {
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Project project)
        {
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Project>> GetAll()
        => await _dbContext.Projects.ToListAsync();

        public async Task<Project> GetProjectById(int id)
        => await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }
}
