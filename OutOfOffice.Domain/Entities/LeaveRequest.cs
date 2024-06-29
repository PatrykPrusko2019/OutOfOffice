
namespace OutOfOffice.Domain.Entities
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public string AbsenceReason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }

        public virtual Employee Employee { get; set; }

        public  int EmployeeId { get; set; }

        public int? ApprovalRequestId { get; set; }

        public virtual ApprovalRequest? Approval_Request { get; set; }
    }
}
