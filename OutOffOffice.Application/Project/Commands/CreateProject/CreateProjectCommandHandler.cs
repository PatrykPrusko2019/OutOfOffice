
using AutoMapper;
using MediatR;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.Employee.Commands.CreateEmployee;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.Project.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateProjectCommandHandler(IProjectRepository projectRepository, IMapper mapper, IUserContext userContext, IEmployeeRepository employeeRepository)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _userContext = userContext;
            _employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            var isEditable = CurrentUser.Role == "ADMIN" || CurrentUser.Role == "PROJECT_MANAGER"; // jesli zalogowany lecz nie ma go na liscie employees, to musi najpierw siebie stworzyc do listy, potem jesli ma role: HR_MANAGER lub ADMIN to ma dostep

            if (!isEditable)
            {
                return Unit.Value;
            }

            var projectManager = _employeeRepository.GetByName(user.Email).Result;

            var project = _mapper.Map<OutOfOffice.Domain.Entities.Project>(request);

            project.ProjectManagerId = projectManager.Id;

            await _projectRepository.Create(project);

            return Unit.Value;
        }
    }
}
