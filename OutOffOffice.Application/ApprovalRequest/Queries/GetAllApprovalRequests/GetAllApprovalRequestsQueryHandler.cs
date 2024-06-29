
using AutoMapper;
using MediatR;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.ApprovalRequest.Queries.GetAllApprovalRequests
{
    public class GetAllApprovalRequestsQueryHandler : IRequestHandler<GetAllApprovalRequestsQuery, IEnumerable<ApprovalRequestDto>>
    {
        private readonly IApprovalRequestRepository _repository;
        private readonly IMapper _mapper;

        public GetAllApprovalRequestsQueryHandler(IApprovalRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApprovalRequestDto>> Handle(GetAllApprovalRequestsQuery request, CancellationToken cancellationToken)
        {
            var approvalRequests = await _repository.GetAll();
            var approvalRequestsDtos = _mapper.Map<IEnumerable<ApprovalRequestDto>>(approvalRequests);

            return approvalRequestsDtos;
        }
    }
}
