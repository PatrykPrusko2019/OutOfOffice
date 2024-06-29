
using MediatR;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.LeaveRequest.Commands.DeleteLeaveRequest
{
    public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand>
    {
        private readonly ILeaveRequestRepository _repository;

        public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _repository.GetLeaveRequestById(request.Id);

            await _repository.Delete(leaveRequest);

            return Unit.Value;
        }
    }
}
