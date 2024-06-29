
using OutOfOffice.Domain.Entities;

namespace OutOfOffice.Domain.Interfaces
{
    public interface IApprovalRequestRepository
    {
        Task Create(ApprovalRequest approvalRequest);
        Task<ApprovalRequest> GetApprovalReguestById(int id);
        Task<ApprovalRequest> GetApprovalReguestByLeaveRequestId(int id);
        Task Commit();
        Task Delete(ApprovalRequest approvalRequest);
        Task<IEnumerable<ApprovalRequest>> GetAll();
    }
}
