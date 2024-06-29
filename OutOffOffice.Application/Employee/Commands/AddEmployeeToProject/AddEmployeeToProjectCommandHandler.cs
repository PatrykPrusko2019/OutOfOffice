
using MediatR;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.Employee.Commands.RemoveEmployee;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.Employee.Commands.AddEmployeeToProject
{
    public class AddEmployeeToProjectCommandHandler : IRequestHandler<AddEmployeeToProjectCommand>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IProjectRepository _projectRepository;

        public AddEmployeeToProjectCommandHandler(IEmployeeRepository repository, IProjectRepository projectRepository)
        {
            _repository = repository;
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(AddEmployeeToProjectCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetEmployeeById(request.Id);

            if (employee.Subdivision == request.Subdivision) return Unit.Value;

            employee.Subdivision = request.Subdivision;

            var project = await _projectRepository.GetProjectById(int.Parse(request.Subdivision));

            project.EmployeesId += request.Id.ToString() + ",";
            
            await _repository.Commit();

            return Unit.Value;
        }
    }
}
