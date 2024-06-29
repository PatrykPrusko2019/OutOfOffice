
namespace OutOffOffice.Application.ApprovalRequest
{
    public class ApprovalRequestDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }

        public int ApproverEmployeeId { get; set; }
        public int LeaveRequestId { get; set; }
    }
}
