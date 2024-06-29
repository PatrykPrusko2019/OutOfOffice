

using MediatR;
using NuGet.Protocol.Core.Types;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.Employee.Commands.EditEmployee;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.ApprovalRequest.Commands.EditAprovalRequest
{
    public class EditApprovalRequestCommandHandler : IRequestHandler<EditApprovalRequestCommand>
    {
        private readonly IApprovalRequestRepository _approvalRequestRepository;
        private readonly IUserContext _userContext;

        public EditApprovalRequestCommandHandler(IApprovalRequestRepository approvalRequestRepository, IUserContext userContext)
        {
            _approvalRequestRepository = approvalRequestRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditApprovalRequestCommand request, CancellationToken cancellationToken)
        {
            var approvalRequest = await _approvalRequestRepository.GetApprovalReguestById(request.Id);

            //var isEditable = CurrentUser.Role == "HR_MANAGER" || CurrentUser.Role == "ADMIN";

            //if (!isEditable)
            //{
            //    return Unit.Value;
            //}

            approvalRequest.Status = request.Status != null ? request.Status : approvalRequest.Status;
            approvalRequest.Comment = request.Comment != null ? request.Comment : approvalRequest.Comment;

            await _approvalRequestRepository.Commit();

            return Unit.Value;
        }
    }
}
