using AutoMapper;
using MediatR;
using OutOffOffice.Application.ApplicationUser;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.Employee.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper, IUserContext userContext)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            var isEditable = (CurrentUser.Role == "ADMIN" || CurrentUser.Role == "HR_MANAGER") || request.FullName.ToLower().Equals(user.Email.ToLower().Trim()); // jesli zalogowany lecz nie ma go na liscie employees, to musi najpierw siebie stworzyc do listy, potem jesli ma role: HR_MANAGER lub ADMIN to ma dostep

            if (!isEditable)
            {
                return Unit.Value;
            }
            request.Subdivision = "";
            request.Photo = "";
            var employee = _mapper.Map<OutOfOffice.Domain.Entities.Employee>(request);

            employee.CreatedById = _userContext.GetCurrentUser().Id;
            employee.IdHrManager = (int)user.EmployeeId;

            await _employeeRepository.Create(employee);

            return Unit.Value;
        }
    }
}
