
using MediatR;
using OutOffOffice.Application.Employee.Queries.GetEmployeeById;
using OutOffOffice.Application.Employee;
using AutoMapper;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.LeaveRequest.Queries.GetLeaveRequestById
{
    public class GetLeaveRequestByIdQueryHandler : IRequestHandler<GetLeaveRequestByIdQuery, LeaveRequestDto>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;

        public GetLeaveRequestByIdQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
        }

        public async Task<LeaveRequestDto> Handle(GetLeaveRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestById(request.Id);

            var leaveRequestDto = _mapper.Map<LeaveRequestDto>(leaveRequest);

            return leaveRequestDto;
        }
    }
}
