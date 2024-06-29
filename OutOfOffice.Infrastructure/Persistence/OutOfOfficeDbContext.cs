
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Domain.Entities;

namespace OutOfOffice.Infrastructure.Persistence
{
    public class OutOfOfficeDbContext : IdentityDbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public OutOfOfficeDbContext(DbContextOptions<OutOfOfficeDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(e => e.Approval_Request)
            .WithOne(e => e.LeaveRequest)
                .HasForeignKey<LeaveRequest>(e => e.ApprovalRequestId)
                .IsRequired(false);
        }
    }
}
