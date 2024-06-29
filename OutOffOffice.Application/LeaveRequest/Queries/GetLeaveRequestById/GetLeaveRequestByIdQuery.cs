
using MediatR;

namespace OutOffOffice.Application.LeaveRequest.Queries.GetLeaveRequestById
{
    public class GetLeaveRequestByIdQuery : IRequest<LeaveRequestDto>
    {
        public int Id { get; set; }

        public GetLeaveRequestByIdQuery(int id)
        {
            Id = id;
        }
    }
}
