
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Domain.Entities;
using OutOfOffice.Domain.Interfaces;
using OutOfOffice.Infrastructure.Persistence;

namespace OutOfOffice.Infrastructure.Repositories
{
    public class ApprovalRequestRepository : IApprovalRequestRepository
    {
        private readonly OutOfOfficeDbContext _dbContext;

        public ApprovalRequestRepository(OutOfOfficeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
            => await _dbContext.SaveChangesAsync();

        public async Task Create(ApprovalRequest approvalRequest)
        {
            _dbContext.ApprovalRequests.Add(approvalRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(ApprovalRequest approvalRequest)
        {
            _dbContext.ApprovalRequests.Remove(approvalRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApprovalRequest>> GetAll()
        => await _dbContext.ApprovalRequests.ToListAsync();

        public async Task<ApprovalRequest> GetApprovalReguestById(int id)
            => await _dbContext.ApprovalRequests.FirstOrDefaultAsync(a => a.Id == id);

        public async Task<ApprovalRequest> GetApprovalReguestByLeaveRequestId(int id)
        => await _dbContext.ApprovalRequests.FirstOrDefaultAsync(a => a.LeaveRequestId == id);
    }
}
