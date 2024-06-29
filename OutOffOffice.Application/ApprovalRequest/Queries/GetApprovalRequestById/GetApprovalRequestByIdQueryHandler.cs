
using AutoMapper;
using MediatR;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.ApprovalRequest.Queries.GetApprovalRequestById
{
    public class GetApprovalRequestByIdQueryHandler : IRequestHandler<GetApprovalRequestByIdQuery, ApprovalRequestDto>
    {
        private readonly IApprovalRequestRepository _repository;
        private readonly IMapper _mapper;

        public GetApprovalRequestByIdQueryHandler(IApprovalRequestRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApprovalRequestDto> Handle(GetApprovalRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var approvalReguest = await _repository.GetApprovalReguestById(request.Id);

            var approvalReguestDto = _mapper.Map<ApprovalRequestDto>(approvalReguest);

            return approvalReguestDto;
        }
    }
}
