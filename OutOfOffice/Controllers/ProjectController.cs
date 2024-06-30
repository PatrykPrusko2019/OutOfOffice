using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.Employee.Commands.EditEmployee;
using OutOffOffice.Application.Employee.Queries.GetAllEmployees;
using OutOffOffice.Application.Employee.Queries.GetEmployeeById;
using OutOffOffice.Application.Project;
using OutOffOffice.Application.Project.Commands.CreateProject;
using OutOffOffice.Application.Project.Commands.DeleteProject;
using OutOffOffice.Application.Project.Commands.EditProject;
using OutOffOffice.Application.Project.Queries.GetAllProjects;
using OutOffOffice.Application.Project.Queries.GetProjectById;
using OutOfOffice.Domain.Entities;
using OutOfOffice.Domain.Interfaces;
using X.PagedList;

namespace OutOfOffice.MVC.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly INotyfService _toastService;
        private readonly IEmployeeRepository _employeeRepository;

        public ProjectController(IMediator mediator, IMapper mapper, IUserContext userContext, INotyfService toastService, IEmployeeRepository employeeRepository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userContext = userContext;
            _toastService = toastService;
            _employeeRepository = employeeRepository;
        }

        [Route("Lists/Projects")]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchProjectId, int? page)
        {
            int pageSize = 7;
            int pageNumber = (page ?? 1);

            var user = _userContext.GetCurrentUser();

            var projects = await _mediator.Send(new GetAllProjectsQuery());
            IEnumerable<ProjectDto> searchProjects;
            if (CurrentUser.Role == "EMPLOYEE")
            {
                int id = (int) user.EmployeeId;
                var employeeDto = await _mediator.Send(new GetEmployeeByIdQuery(id));
                searchProjects = projects.Where(p => p.Id.ToString() == employeeDto.Subdivision);
            }
            else if (CurrentUser.Role == "HR_MANAGER")
            {
                int id = (int)user.EmployeeId;
                var employees = await _mediator.Send(new GetAllEmployeesQuery());
                employees = employees.Where(e => e.IdHrManager == id);
                List<ProjectDto> results = new List<ProjectDto>();
                for (int i = 0; i < employees.Count(); i++)
                {
                    var subdivision = employees.ElementAt(i).Subdivision;
                    var searchProject = projects.FirstOrDefault(p => p.Id.ToString() == subdivision);
                    if (searchProject != null && !results.Contains(searchProject))  results.Add(searchProject);
                }
                searchProjects = results;
            }
            else if (CurrentUser.Role == "PROJECT_MANAGER")
            {
                searchProjects = projects.Where(p => p.ProjectManagerId == user?.EmployeeId);
            }
            else
            {
                //ADMIN for all projects access
                searchProjects = projects;
            }

            if (searchProjects != null)
            {
                if (searchProjectId != null)
                {
                    page = 1;
                }
                else
                {
                    searchProjectId = currentFilter;
                }

                ViewBag.CurrentFilter = searchProjectId;

                if (!String.IsNullOrEmpty(searchProjectId))
                {
                    searchProjects = FindProjectsByProjectId(searchProjectId, projects);                      
                }

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "projectType_desc" : "";
                ViewBag.DateSortParm = sortOrder == "StartDate" ? "startDate_desc" : "StartDate";
                ViewBag.DateSortParm = sortOrder == "EndDate" ? "endDate_desc" : "EndDate";
                ViewBag.NameSortParm = sortOrder == "Comment" ? "comment_desc" : "Comment";
                ViewBag.NameSortParm = sortOrder == "Status" ? "status_desc" : "Status";
                ViewBag.NameSortParm = sortOrder == "Id" ? "id_desc" : "Id";

                switch (sortOrder)
                {
                    case "projectType_desc":
                        searchProjects = searchProjects.OrderByDescending(s => s.ProjectType);
                        break;
                    case "StartDate":
                        searchProjects = searchProjects.OrderBy(s => s.StartDate);
                        break;
                    case "startDate_desc":
                        searchProjects = searchProjects.OrderByDescending(s => s.StartDate);
                        break;
                    case "EndDate":
                        searchProjects = searchProjects.OrderBy(s => s.EndDate);
                        break;
                    case "endDate_desc":
                        searchProjects = searchProjects.OrderByDescending(s => s.EndDate);
                        break;
                    case "Comment":
                        searchProjects = searchProjects.OrderBy(s => s.Comment);
                        break;
                    case "comment_desc":
                        searchProjects = searchProjects.OrderByDescending(s => s.Comment);
                        break;
                    case "":
                        searchProjects = searchProjects.OrderBy(s => s.Status);
                        break;
                    case "status_desc":
                        searchProjects = searchProjects.OrderByDescending(s => s.Status);
                        break;
                    case "Id":
                        searchProjects = searchProjects.OrderBy(s => s.Id);
                        break;
                    case "id_desc":
                        searchProjects = searchProjects.OrderByDescending(s => s.Id);
                        break;
                    default:
                        searchProjects = searchProjects.OrderBy(s => s.ProjectType);
                        break;
                }


                return View(searchProjects.ToPagedList(pageNumber, pageSize));
            }

            
            return View(searchProjects.ToPagedList(pageNumber, pageSize));
        }

        private IEnumerable<ProjectDto> FindProjectsByProjectId(string searchProjectId, IEnumerable<ProjectDto> projects)
        {
            if (!String.IsNullOrEmpty(searchProjectId))
            {
                var searchedEmployee = projects.Where(s => searchProjectId == s.Id.ToString()).ToList();
                return searchedEmployee;
            }
            return projects;
        }

        public IActionResult Create()
        {
            //if (User.Identity == null || !User.Identity.IsAuthenticated)
            //{
            //    return RedirectToPage("/Account/Login", new { area = "Identity" });
            //}

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectCommand project)
        {

            if (!ModelState.IsValid)
            {
                _toastService.Error("no Project added");
                return View(project);
            }
            _toastService.Success("Added new Project");
            await _mediator.Send(project);
            return RedirectToAction(nameof(Index));
        }

        [Route("Project/{id}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var projectDto = await _mediator.Send(new GetProjectByIdQuery(id));

            //if (!projectDto.IsEditable)
            //{
            //    return RedirectToAction("NoAccess", "Home");
            //}

            EditProjectCommand model = _mapper.Map<EditProjectCommand>(projectDto);

            return View(model);
        }

        [HttpPost]
        [Route("Project/{id}/Edit")]
        public async Task<IActionResult> Edit(int id, EditProjectCommand project)
        {

            if (!ModelState.IsValid)
            {
                _toastService.Error("no Project updated");
                return View(project);
            }
            _toastService.Success("Updated given Project");
            await _mediator.Send(project);
            return RedirectToAction(nameof(Index));
        }

        [Route("Project/{id}/Details")]
        public async Task<IActionResult> Details(int id)
        {
            var projectDto = await _mediator.Send(new GetProjectByIdQuery(id));
            return View(projectDto);
        }

        [Route("Project/{id}/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var givenEmployees = await _mediator.Send(new GetAllEmployeesQuery());

            givenEmployees = givenEmployees.Where(x => x.Subdivision == id.ToString()).ToArray();

            for(int i = 0; i < givenEmployees.Count(); i++)
            {
                var updatedEmployee = givenEmployees.ElementAt(i);
                updatedEmployee.Subdivision = "";
                EditEmployeeCommand model = _mapper.Map<EditEmployeeCommand>(updatedEmployee);
                await _mediator.Send(model);
            }

            await _mediator.Send(new DeleteProjectCommand(id));
            _toastService.Success("Deleted given Project");
            return RedirectToAction(nameof(Index));
        }

    }
}
