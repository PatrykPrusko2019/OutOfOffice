
using MediatR;
using OutOffOffice.Application.ApplicationUser;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.Employee.Commands.EditEmployee
{
    public class EditEmployeeCommandHandler : IRequestHandler<EditEmployeeCommand>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUserContext _userContext;

        public EditEmployeeCommandHandler(IEmployeeRepository repository, IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repository.GetEmployeeById(request.Id);

            var user = _userContext.GetCurrentUser();

            bool result = employee.Subdivision != request.Subdivision;

            if ( !(result && request.Subdivision == "" || result && request.Subdivision == null) )
            {
                var isEditable = user != null && employee.IdHrManager == user.EmployeeId && (employee.FullName.ToLower().Trim() != user.Email.Trim());
                isEditable = isEditable && (CurrentUser.Role == "HR_MANAGER" || CurrentUser.Role == "ADMIN");

                if (!isEditable)
                {
                    return Unit.Value;
                }
            }

            employee.Status = request.Status != null ? request.Status : employee.Status;
            employee.OutOfOfficeBalance = request.OutOfOfficeBalance;
            employee.Position = request.Position != null ? request.Position : employee.Position;
            employee.Photo = request.Photo != null ? employee.Photo = request.Photo : employee.Photo = "";
            employee.Subdivision = request.Subdivision != null ? request.Subdivision : employee.Subdivision;

            await _repository.Commit();

            return Unit.Value;
        }
    }
}
