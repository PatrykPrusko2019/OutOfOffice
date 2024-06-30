
using MediatR;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.Employee.Commands.EditEmployee;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.Project.Commands.EditProject
{
    public class EditProjectCommandHandler : IRequestHandler<EditProjectCommand>
    {
        private readonly IProjectRepository _repository;
        private readonly IUserContext _userContext;

        public EditProjectCommandHandler(IProjectRepository repository, IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetProjectById(request.Id);
            
            if ( !(project.EmployeesId != request.EmployeesId) )
            {
                var user = _userContext.GetCurrentUser();
                var isEditable = user.EmployeeId == request.ProjectManagerId;
                isEditable = isEditable && (CurrentUser.Role == "PROJECT_MANAGER" || CurrentUser.Role == "ADMIN");

                if (!isEditable)
                {
                    return Unit.Value;
                }
            }

            project.ProjectType = request.ProjectType != null ? request.ProjectType : project.ProjectType;
            project.StartDate = request.StartDate;
            project.EndDate = request.EndDate;
            project.Comment = request.Comment != null ? request.Comment : project.Comment;
            project.Status = request.Status != null ? request.Status : project.Status;
            project.EmployeesId = request.EmployeesId;

            await _repository.Commit();

            return Unit.Value;
        }
    }
}
