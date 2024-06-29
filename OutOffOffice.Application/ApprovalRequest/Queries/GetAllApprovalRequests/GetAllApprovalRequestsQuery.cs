
using MediatR;

namespace OutOffOffice.Application.ApprovalRequest.Queries.GetAllApprovalRequests
{
    public class GetAllApprovalRequestsQuery : IRequest<IEnumerable<ApprovalRequestDto>>
    {
    }
}
