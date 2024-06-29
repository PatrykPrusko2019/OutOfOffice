
using AutoMapper;
using MediatR;
using NuGet.Protocol.Core.Types;
using OutOffOffice.Application.ApprovalRequest;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.LeaveRequest.Queries.GetAllLEaveRequests
{
    public class GetAllLEaveRequestsQueryHandler : IRequestHandler<GetAllLEaveRequestsQuery, IEnumerable<LeaveRequestDto>>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;

        public GetAllLEaveRequestsQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LeaveRequestDto>> Handle(GetAllLEaveRequestsQuery request, CancellationToken cancellationToken)
        {
            var leaveRequests = await _leaveRequestRepository.GetAll();
            var leaveRequestsDtos = _mapper.Map<IEnumerable<LeaveRequestDto>>(leaveRequests);

            return leaveRequestsDtos;
        }
    }
}
