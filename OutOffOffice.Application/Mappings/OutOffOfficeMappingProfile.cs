using AutoMapper;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.ApprovalRequest;
using OutOffOffice.Application.ApprovalRequest.Commands.EditAprovalRequest;
using OutOffOffice.Application.Employee;
using OutOffOffice.Application.Employee.Commands.EditEmployee;
using OutOffOffice.Application.LeaveRequest;
using OutOffOffice.Application.LeaveRequest.Commands.EditLeaveRequest;
using OutOffOffice.Application.Project;
using OutOffOffice.Application.Project.Commands.EditProject;

namespace OutOffOffice.Application.Mappings
{
    public class OutOffOfficeMappingProfile : Profile
    {
        public OutOffOfficeMappingProfile(IUserContext userContext)
        {
            var user = userContext.GetCurrentUser();

            CreateMap<EmployeeDto, OutOfOffice.Domain.Entities.Employee>();

            CreateMap<OutOfOffice.Domain.Entities.Employee, EmployeeDto>()
                .ForMember(dto => dto.IsEditable, opt => opt.MapFrom(src => user != null && src.IdHrManager == user.EmployeeId));

            CreateMap<EmployeeDto, EditEmployeeCommand>();

            CreateMap<LeaveRequestDto, EditLeaveRequestCommand>();

            CreateMap<ApprovalRequestDto, EditApprovalRequestCommand>();

            CreateMap<ProjectDto, EditProjectCommand>();

            CreateMap<LeaveRequestDto, OutOfOffice.Domain.Entities.LeaveRequest>();

            CreateMap<OutOfOffice.Domain.Entities.LeaveRequest, LeaveRequestDto>();

            CreateMap<ApprovalRequestDto, OutOfOffice.Domain.Entities.ApprovalRequest>();

            CreateMap<OutOfOffice.Domain.Entities.ApprovalRequest, ApprovalRequestDto>();

            CreateMap<ProjectDto, OutOfOffice.Domain.Entities.Project>();

            CreateMap<OutOfOffice.Domain.Entities.Project, ProjectDto>();

        }
    }
}
