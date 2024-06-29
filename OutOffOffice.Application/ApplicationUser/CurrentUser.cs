

namespace OutOffOffice.Application.ApplicationUser
{
    public class CurrentUser
    {
        public CurrentUser(string id, string email, int employeeId, string status)
        {
            Id = id;
            Email = email;
            EmployeeId = employeeId;
            Role = status;
        }

        public string Id { get; set; }
        public string? Email { get; set; }
        public int? EmployeeId { get; set; }
        public static string? Role { get; set; }
        public static int IdEmployeeToProject { get; set; }
    }
}
