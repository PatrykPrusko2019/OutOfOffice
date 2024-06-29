
using MediatR;
using OutOffOffice.Application.Employee.Commands.RemoveEmployee;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.ApprovalRequest.Commands.DeleteApprovalRequest
{
    public class DeleteApprovalRequestCommandHandler : IRequestHandler<DeleteApprovalRequestCommand>
    {
        private readonly IApprovalRequestRepository _repository;

        public DeleteApprovalRequestCommandHandler(IApprovalRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteApprovalRequestCommand request, CancellationToken cancellationToken)
        {
            var deletedApprovalRequest = await _repository.GetApprovalReguestById(request.Id);

            await _repository.Delete(deletedApprovalRequest);

            return Unit.Value;
        }
    }
}
