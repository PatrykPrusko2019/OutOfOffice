
using AutoMapper;
using MediatR;
using OutOffOffice.Application.ApplicationUser;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.ApprovalRequest.Commands.CreateApprovalRequest
{
    public class CreateApprovalRequestCommandHandler : IRequestHandler<CreateApprovalRequestCommand>
    {
        private readonly IApprovalRequestRepository _approvalRequestRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateApprovalRequestCommandHandler(IApprovalRequestRepository approvalRequestRepository, IMapper mapper, IUserContext userContext)
        {
            _approvalRequestRepository = approvalRequestRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateApprovalRequestCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            var isEditable = CurrentUser.Role == "EMPLOYEE";

            if (!isEditable)
            {
                return Unit.Value;
            }


            var approvalRequest = _mapper.Map<OutOfOffice.Domain.Entities.ApprovalRequest>(request);



            await _approvalRequestRepository.Create(approvalRequest);

            return Unit.Value;
        }
    }
}
