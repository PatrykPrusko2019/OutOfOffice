
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Domain.Entities;
using OutOfOffice.Domain.Interfaces;
using OutOfOffice.Infrastructure.Persistence;

namespace OutOfOffice.Infrastructure.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly OutOfOfficeDbContext _dbContext;

        public LeaveRequestRepository(OutOfOfficeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
            => await _dbContext.SaveChangesAsync();

        public async Task Create(LeaveRequest leaveRequest)
        {
            _dbContext.LeaveRequests.Add(leaveRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(LeaveRequest leaveRequest)
        {
            _dbContext.LeaveRequests.Remove(leaveRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetAll()
        => await _dbContext.LeaveRequests.ToListAsync();

        public async Task<LeaveRequest> GetLeaveRequestById(int id)
        => await _dbContext.LeaveRequests.FirstOrDefaultAsync(l => l.Id == id);
    }
}
