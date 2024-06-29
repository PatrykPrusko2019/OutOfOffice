
using MediatR;
using OutOffOffice.Application.ApprovalRequest;

namespace OutOffOffice.Application.LeaveRequest.Queries.GetAllLEaveRequests
{
    public class GetAllLEaveRequestsQuery : IRequest<IEnumerable<LeaveRequestDto>>
    {
    }
}
