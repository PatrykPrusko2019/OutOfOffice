
using Microsoft.AspNetCore.Identity;

namespace OutOfOffice.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? Subdivision { get; set; } // kind of Project
        public string Position { get; set; }
        public string Status { get; set; }
        public int OutOfOfficeBalance { get; set; }
        public string Photo { get; set; }
        public int IdHrManager { get; set; }

        public virtual List<Project> Projects { get; set; } = new List<Project>();
        public virtual List<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public string? CreatedById { get; set; }
        public IdentityUser? CreatedBy { get; set; }
    }
}
