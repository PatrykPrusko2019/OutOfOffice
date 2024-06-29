
using OutOfOffice.Domain.Entities;

namespace OutOfOffice.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task Create(Employee employee);
        Task<Employee?> GetByName(string name);
        Task<IEnumerable<Employee>> GetAll();
        Task<Employee> GetEmployeeById(int id);
        Task Commit();
        Task Delete(Employee employee);
    }
}
