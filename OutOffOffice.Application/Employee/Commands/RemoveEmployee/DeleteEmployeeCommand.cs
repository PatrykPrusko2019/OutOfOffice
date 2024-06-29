using MediatR;

namespace OutOffOffice.Application.Employee.Commands.RemoveEmployee
{
    public class DeleteEmployeeCommand : OutOfOffice.Domain.Entities.Employee, IRequest
    {
        public DeleteEmployeeCommand(int id)
        {
            Id = id;
        }
    }
}
