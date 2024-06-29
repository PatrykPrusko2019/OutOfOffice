namespace OutOffOffice.Application.LeaveRequest
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public string AbsenceReason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }

        public int EmployeeId { get; set; }

        public int? ApprovalRequestId { get; set; }
    }
}

