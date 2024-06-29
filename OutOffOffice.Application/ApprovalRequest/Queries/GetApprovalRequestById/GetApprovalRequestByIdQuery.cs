
using MediatR;

namespace OutOffOffice.Application.ApprovalRequest.Queries.GetApprovalRequestById
{
    public class GetApprovalRequestByIdQuery : IRequest<ApprovalRequestDto>
    {
        public int Id { get; set; }

        public GetApprovalRequestByIdQuery(int id)
        {
            Id = id;
        }
    }
}
