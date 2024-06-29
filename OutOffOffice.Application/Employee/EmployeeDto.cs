
using OutOffOffice.Application.LeaveRequest;
using OutOffOffice.Application.Project;
using System.Web.Mvc;

namespace OutOffOffice.Application.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Subdivision { get; set; } // kind of Project
        public string? Position { get; set; }
        public string Status { get; set; }
        public int IdHrManager { get; set; }
        public int OutOfOfficeBalance { get; set; }

        [AllowHtml]
        public string? Photo { get; set; }
        public bool IsEditable { get; set; }
        public string? CreatedById { get; set; }

        public List<LeaveRequestDto> LeaveRequests { get; set; }
        public List<ProjectDto> Projects { get; set; }
    }
}
