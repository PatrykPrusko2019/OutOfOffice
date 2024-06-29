using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using OutOffOffice.Application.Extensions;
using OutOfOffice.Infrastructure.Extensions;
using OutOfOffice.Infrastructure.Seeders;


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            var app = builder.Build();

            var scope = app.Services.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredService<OutOfOfficeSeeder>();

            await seeder.Seed();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.UseNotyf();
    
            app.Run();
        
    
