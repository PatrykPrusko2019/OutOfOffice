
using MediatR;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.Employee.Commands.RemoveEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IEmployeeRepository _repository;

        public DeleteEmployeeCommandHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetEmployeeById(request.Id);

            await _repository.Delete(employee);

            return Unit.Value;
        }
    }
}
