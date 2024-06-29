using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutOfOffice.Domain.Interfaces;
using OutOfOffice.Infrastructure.Persistence;
using OutOfOffice.Infrastructure.Repositories;
using OutOfOffice.Infrastructure.Seeders;


namespace OutOfOffice.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OutOfOfficeDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("OutOfOfficeContext"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }));

            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<OutOfOfficeDbContext>();

            services.AddScoped<OutOfOfficeSeeder>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

            services.AddScoped<IApprovalRequestRepository, ApprovalRequestRepository>();

            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        }
    }
}
