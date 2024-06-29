
using MediatR;

namespace OutOffOffice.Application.Project.Commands.DeleteProject
{
    public class DeleteProjectCommand : OutOfOffice.Domain.Entities.Employee, IRequest
    {
        public DeleteProjectCommand(int id)
        {
            Id = id;
        }
    }
}
