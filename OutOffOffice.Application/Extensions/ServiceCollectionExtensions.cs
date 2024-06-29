using AspNetCoreHero.ToastNotification;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.ApprovalRequest.Commands.CreateApprovalRequest;
using OutOffOffice.Application.Employee.Commands.CreateEmployee;
using OutOffOffice.Application.LeaveRequest.Commands.CreateLeaveRequest;
using OutOffOffice.Application.Mappings;
using OutOffOffice.Application.Project.Commands.CreateProject;

namespace OutOffOffice.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();

            services.AddMediatR(typeof(CreateEmployeeCommand));
            services.AddMediatR(typeof(CreateLeaveRequestCommand));
            services.AddMediatR(typeof(CreateApprovalRequestCommand));
            services.AddMediatR(typeof(CreateProjectCommand));

            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                var scope = provider.CreateScope();
                var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();
                cfg.AddProfile(new OutOffOfficeMappingProfile(userContext));
            }).CreateMapper()
            );

            services.AddAutoMapper(typeof(OutOffOfficeMappingProfile));

            services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

                services.AddValidatorsFromAssemblyContaining<CreateLeaveRequestCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddNotyf(config => { config.DurationInSeconds = 3; config.IsDismissable = true; config.Position = NotyfPosition.TopCenter; });

        }
    }
}


