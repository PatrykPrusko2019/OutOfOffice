
namespace OutOfOffice.Domain.Entities
{
    public class ApprovalRequest
    {
        public int Id { get; set; }
        
        public string Status { get; set; }
        public string Comment { get; set; }

        public int ApproverEmployeeId { get; set; }
        public int? LeaveRequestId { get; set; }
        public virtual LeaveRequest? LeaveRequest { get; set; }


    }
}
