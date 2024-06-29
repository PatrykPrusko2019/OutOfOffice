
using MediatR;

namespace OutOffOffice.Application.LeaveRequest.Commands.DeleteLeaveRequest
{
    public class DeleteLeaveRequestCommand : OutOfOffice.Domain.Entities.LeaveRequest, IRequest
    {
        public DeleteLeaveRequestCommand(int id)
        {
            Id = id;
        }
    }
}
