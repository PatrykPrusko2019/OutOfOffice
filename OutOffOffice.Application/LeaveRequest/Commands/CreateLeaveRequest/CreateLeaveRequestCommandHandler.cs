
using AutoMapper;
using MediatR;
using OutOffOffice.Application.ApplicationUser;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper, IUserContext userContext)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            var isEditable = CurrentUser.Role == "EMPLOYEE";

            if (!isEditable)
            {
                return Unit.Value;
            }

            request.Status = "NEW";

            var leaveRequest = _mapper.Map<OutOfOffice.Domain.Entities.LeaveRequest>(request);

            var id = _userContext.GetCurrentUser()?.EmployeeId;
            leaveRequest.EmployeeId = (int) id;
            await _leaveRequestRepository.Create(leaveRequest);

            return Unit.Value;
        }
    }
}
