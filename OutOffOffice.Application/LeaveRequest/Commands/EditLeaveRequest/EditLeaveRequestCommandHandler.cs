
using MediatR;
using OutOffOffice.Application.ApplicationUser;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.LeaveRequest.Commands.EditLeaveRequest
{
    public class EditLeaveRequestCommandHandler : IRequestHandler<EditLeaveRequestCommand>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IUserContext _userContext;

        public EditLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IUserContext userContext)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestById(request.Id);

            //var isEditable = CurrentUser.Role == "HR_MANAGER" || CurrentUser.Role == "ADMIN";

            //if (!isEditable)
            //{
            //    return Unit.Value;
            //}
            leaveRequest.StartDate = request.StartDate;
            leaveRequest.EndDate = request.EndDate;
            leaveRequest.AbsenceReason = request.AbsenceReason != null ? request.AbsenceReason : leaveRequest.AbsenceReason;
            leaveRequest.Comment = request.Comment != null ? request.Comment : leaveRequest.Comment;
            leaveRequest.Status = request.Status != null ? request.Status : leaveRequest.Status;

            await _leaveRequestRepository.Commit();

            return Unit.Value;
        }
    }
}
