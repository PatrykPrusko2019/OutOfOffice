
namespace OutOffOffice.Application.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string ProjectType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public int ProjectManagerId { get; set; } //EmployeeId
        public string? EmployeesId { get; set; }
    }
}
