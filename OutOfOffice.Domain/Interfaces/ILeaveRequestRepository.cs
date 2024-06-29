
using OutOfOffice.Domain.Entities;

namespace OutOfOffice.Domain.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task Create(LeaveRequest leaveRequest);

        Task<LeaveRequest> GetLeaveRequestById(int id);
        Task Commit();
        Task Delete(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetAll();

    }
}
