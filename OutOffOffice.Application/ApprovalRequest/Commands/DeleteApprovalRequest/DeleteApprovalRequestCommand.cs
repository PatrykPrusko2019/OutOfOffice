
using MediatR;

namespace OutOffOffice.Application.ApprovalRequest.Commands.DeleteApprovalRequest
{
    public class DeleteApprovalRequestCommand : OutOfOffice.Domain.Entities.ApprovalRequest, IRequest
    {
        public DeleteApprovalRequestCommand(int id)
        {
            Id = id;
        }
    }
}
