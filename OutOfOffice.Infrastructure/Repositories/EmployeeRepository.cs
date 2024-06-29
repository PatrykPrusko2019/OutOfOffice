using Microsoft.EntityFrameworkCore;
using OutOfOffice.Domain.Entities;
using OutOfOffice.Domain.Interfaces;
using OutOfOffice.Infrastructure.Persistence;

namespace OutOfOffice.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly OutOfOfficeDbContext _dbContext;

        public EmployeeRepository(OutOfOfficeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        => await _dbContext.SaveChangesAsync();

        public async Task Create(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Employee employee)
        {
           _dbContext.Employees.Remove(employee);
           await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAll()
        => await _dbContext.Employees.Include(l => l.LeaveRequests).Include(p => p.Projects).ToListAsync();

        public async Task<Employee?> GetByName(string name)
        => await _dbContext.Employees.FirstOrDefaultAsync(e => e.FullName.ToLower().Replace(" ", string.Empty) == name.ToLower().Trim());

        public async Task<Employee> GetEmployeeById(int id)
        => await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
    }
}
