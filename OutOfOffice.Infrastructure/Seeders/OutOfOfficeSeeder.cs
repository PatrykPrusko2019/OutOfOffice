using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Infrastructure.Persistence;
using System.Data;

namespace OutOfOffice.Infrastructure.Seeders
{
    public class OutOfOfficeSeeder
    {
        private readonly OutOfOfficeDbContext _dbContext;

        public OutOfOfficeSeeder(OutOfOfficeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (! await _dbContext.Database.CanConnectAsync())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }


                if (! _dbContext.Employees.Any()) 
                {
                    var employee0 = new Domain.Entities.Employee()
                    {
                        FullName = "Admin",
                        Position = "ADMIN",
                        Status = "Active",
                        OutOfOfficeBalance = 26,
                        Photo = @$"~/images/men.jpg",
                        
                    };

                    var employee1 = new Domain.Entities.Employee()
                    {
                        FullName = "Patryk Prusko",
                        Position = "HR_MANAGER",
                        Status = "Active",
                        OutOfOfficeBalance = 22,
                        Photo = @$"~/images/mend2.jpg",
                        
                    };

                    var employee2 = new Domain.Entities.Employee()
                    {
                        FullName = "Maciej Mist",
                        Position = "HR_MANAGER",
                        Status = "Active",
                        OutOfOfficeBalance = 26,
                        Photo = @$"~/images/men.png",
                        
                       
                    };

                    var employee3 = new Domain.Entities.Employee()
                    {
                        FullName = "Anna Madra",
                        Position = "PROJECT_MANAGER",
                        Status = "Active",
                        OutOfOfficeBalance = 23,
                        Photo = @$"~/images/fifth.jpg",
                        Projects = new List<Domain.Entities.Project>()
                        {
                            new Domain.Entities.Project()
                            {
                                ProjectType = "Systems of Linux",
                                Comment = "Administrations ",
                                StartDate = DateTime.Now,
                                EndDate = new DateTime(2026,1,1),
                                Status = "Active"
                                
                            },
                            new Domain.Entities.Project()
                            {
                                ProjectType = "Systems of IOS",
                                Comment = "Administrations OIS",
                                StartDate = DateTime.Now,
                                EndDate = new DateTime(2030,2,3),
                                Status = "Active",
                            }
                        }
                    };

                    var employee4 = new Domain.Entities.Employee()
                    {
                        FullName = "Maciej Next",
                        Position = "PROJECT_MANAGER",
                        Status = "Active",
                        OutOfOfficeBalance = 23,
                        Photo = @$"~/images/mend2.png",
                        Projects = new List<Domain.Entities.Project>()
                        {
                            new Domain.Entities.Project()
                            {
                                ProjectType = "Programming AI",
                                Comment = "Programming something ",
                                StartDate = DateTime.Now,
                                EndDate = new DateTime(2025,2,1),
                                Status = "Active"

                            },
                            new Domain.Entities.Project()
                            {
                                ProjectType = "Algoritms",
                                Comment = "Something",
                                StartDate = DateTime.Now,
                                EndDate = new DateTime(2027,2,3),
                                Status = "Active",
                            }
                        }
                    };

                    var employee5 = new Domain.Entities.Employee()
                    {
                        FullName = "Maciej Nextewe",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 23,
                        Photo = @$"~/images/mend2.jpg",
                        
                    };

                    var employee6 = new Domain.Entities.Employee()
                    {
                        FullName = "Blazej Ananas",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 23,
                        Photo = @$"~/images/men.jpg",

                    };

                    var employee7 = new Domain.Entities.Employee()
                    {
                        FullName = "Andrzej An",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 23,
                        Photo = @$"~/images/mend2.jpg",

                    };

                    var employee8 = new Domain.Entities.Employee()
                    {
                        FullName = "Szymon Much",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 23,
                        Photo = @$"~/images/men.jpg",

                    };

                    var employee9 = new Domain.Entities.Employee()
                    {
                        FullName = "Alicja Ajla",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 23,
                        Photo = @$"~/images/photo.png",

                    };

                    var employee10 = new Domain.Entities.Employee()
                    {
                        FullName = "Patrycja Blaszczak",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 28,
                        Photo = @$"~/images/second.jpg",

                    };

                    var employee11 = new Domain.Entities.Employee()
                    {
                        FullName = "Maciej Nextewe",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 23,
                        Photo = @$"~/images/mend2.jpg",

                    };

                    var employee12 = new Domain.Entities.Employee()
                    {
                        FullName = "Marianna Oleksiej",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 13,
                        Photo = @$"~/images/fifth.jpg",

                    };

                    var employee13 = new Domain.Entities.Employee()
                    {
                        FullName = "Dagmara Ajon",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 14,
                        Photo = @$"~/images/third.jpg",

                    };

                    var employee14 = new Domain.Entities.Employee()
                    {
                        FullName = "Sandra Ptak",
                        Position = "EMPLOYEE",
                        Status = "Active",
                        OutOfOfficeBalance = 9,
                        Photo = @$"~/images/fifth.jpg",

                    };

                    employee0.IdHrManager = 1;
                    employee1.IdHrManager = 2;
                    employee2.IdHrManager = 3;
                    employee3.IdHrManager = 4;
                    employee4.IdHrManager = 5;

                    employee5.IdHrManager = 2;
                    employee6.IdHrManager = 2;
                    employee7.IdHrManager = 2;
                    employee8.IdHrManager = 2;
                    employee9.IdHrManager = 2;
                    employee10.IdHrManager = 2;
                    employee11.IdHrManager = 3;
                    employee12.IdHrManager = 3;
                    employee13.IdHrManager = 3;
                    employee14.IdHrManager = 3;

                    _dbContext.Employees.Add(employee0);
                    _dbContext.Employees.Add(employee1);
                    _dbContext.Employees.Add(employee2);
                    _dbContext.Employees.Add(employee3);
                    _dbContext.Employees.Add(employee4);
                    _dbContext.Employees.Add(employee5);
                    _dbContext.Employees.Add(employee6);
                    _dbContext.Employees.Add(employee7);
                    _dbContext.Employees.Add(employee8);
                    _dbContext.Employees.Add(employee9);
                    _dbContext.Employees.Add(employee10);
                    _dbContext.Employees.Add(employee11);
                    _dbContext.Employees.Add(employee12);
                    _dbContext.Employees.Add(employee13);
                    _dbContext.Employees.Add(employee14);

                    await _dbContext.SaveChangesAsync();

                    var user = _dbContext.Employees.FirstOrDefault(u => u.FullName == "Szymon Much");
                    var user2 = _dbContext.Employees.FirstOrDefault(u => u.FullName == "Alicja Ajla");
                    var manager = _dbContext.Employees.FirstOrDefault(u => u.FullName == "Anna Madra");

                    manager.Projects.ElementAt(0).EmployeesId += user.Id + ",";
                    user.Subdivision = manager.Projects.ElementAt(0).Id.ToString();

                    manager.Projects.ElementAt(1).EmployeesId += user2.Id + ",";
                    user2.Subdivision = manager.Projects.ElementAt(1).Id.ToString();

                    _dbContext.Employees.UpdateRange(user, user2, manager);


                    await _dbContext.SaveChangesAsync();
                }
            }

        }
    }
}
