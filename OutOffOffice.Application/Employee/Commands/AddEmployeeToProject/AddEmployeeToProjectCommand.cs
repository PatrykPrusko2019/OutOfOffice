
using MediatR;

namespace OutOffOffice.Application.Employee.Commands.AddEmployeeToProject
{
    public class AddEmployeeToProjectCommand : OutOfOffice.Domain.Entities.Employee, IRequest
    {
        public AddEmployeeToProjectCommand(int employeeId, int projectId)
        {
            Id = employeeId;
            Subdivision = projectId.ToString();
        }
    }
}
