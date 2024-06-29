using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.Employee;
using OutOffOffice.Application.Employee.Commands.AddEmployeeToProject;
using OutOffOffice.Application.Employee.Commands.CreateEmployee;
using OutOffOffice.Application.Employee.Commands.EditEmployee;
using OutOffOffice.Application.Employee.Commands.RemoveEmployee;
using OutOffOffice.Application.Employee.Queries.GetAllEmployees;
using OutOffOffice.Application.Employee.Queries.GetEmployeeById;
using OutOffOffice.Application.Project.Queries.GetAllProjects;

namespace OutOfOffice.MVC.Controllers
{
    
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly INotyfService _toastService;

        
        public EmployeeController(IMediator mediator, IMapper mapper, IUserContext userContext, INotyfService notyf)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userContext = userContext;
            _toastService = notyf;
        }

        [Route("Lists/Employees")]
        public async Task<IActionResult> Index(string searchName)
        {
            if (CurrentUser.Role == "EMPLOYEE") return View();

            var user = _userContext.GetCurrentUser();

            var employees = await _mediator.Send(new GetAllEmployeesQuery());

            var searchEmployye = employees.FirstOrDefault(e => e.FullName.Replace(" ", string.Empty).ToLower().Equals(user.Email?.ToLower()));

            if (searchEmployye != null && CurrentUser.Role != "PROJECT_MANAGER" && CurrentUser.Role != "ADMIN")
            {
                var givenEmployees = employees.Where(e => e.IdHrManager == user.EmployeeId).ToList();

               return View(FindGivenEmployee(searchName, givenEmployees));
            }
            else if (CurrentUser.Role == "PROJECT_MANAGER")
            {
                    employees = employees.Where(e => e.Subdivision == null);
                
                return View(FindGivenEmployee(searchName, employees));
            }

            return View(FindGivenEmployee(searchName, employees));
        }

        private IEnumerable<EmployeeDto> FindGivenEmployee(string searchName, IEnumerable<EmployeeDto> employees)
        {
            if (!String.IsNullOrEmpty(searchName))
            {
                var searchedEmployee = employees.Where(s => s.FullName.ToLower().Trim().Contains(searchName.ToLower().Trim())).ToList();
                return searchedEmployee;
            }
            return employees;
        }

        [Route("Employee/{id}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var employeeDto = await _mediator.Send(new GetEmployeeByIdQuery(id));

            if (!employeeDto.IsEditable)
            {
                _toastService.Error("no employee updated");
                return RedirectToAction("NoAccess", "Home");
            }

            EditEmployeeCommand model = _mapper.Map<EditEmployeeCommand>(employeeDto);

            return View(model);
        }

        [HttpPost]
        [Route("Employee/{id}/Edit")]
        public async Task<IActionResult> Edit(int id ,EditEmployeeCommand employee)
        {

            if (!ModelState.IsValid)
            {
                _toastService.Error("no employee updated");
                return View(employee);
            }

            await _mediator.Send(employee);
            _toastService.Success("Updated given Employee");
            return RedirectToAction(nameof(Index));
        }

        [Route("Employee/{id}/Details")]
        public async Task<IActionResult> Details(int id)
        {
            var employeeDto = await _mediator.Send(new GetEmployeeByIdQuery(id));
            return View(employeeDto);
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
        public async Task<IActionResult> Create(CreateEmployeeCommand employee)
        {

            if(! ModelState.IsValid ) 
            {
                _toastService.Error("no employee added");
                return View(employee);
            }

            await _mediator.Send(employee);
            _toastService.Success("Added new Employee");
            return RedirectToAction(nameof(Index));
        }

        [Route("Employee/{id}/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteEmployeeCommand(id));
            _toastService.Success("Deleted given Employee");
            return RedirectToAction(nameof(Index));
        }

        
        [Route("Employee/{id}/AddToProject")]
        public async Task<IActionResult> AddToProject(int id)
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());

            var projectsGivenProjectManager = projects.Where(p => p.ProjectManagerId == _userContext.GetCurrentUser().EmployeeId);

            //if (projectsGivenProjectManager != null)
            //{
            //    projectsGivenProjectManager.First().EmployeesId = projectsGivenProjectManager.First().EmployeesId + "," + id;
            //}

            CurrentUser.IdEmployeeToProject = id;

            return View(projectsGivenProjectManager);
        }

        [Route("Employee/{id}/AddEmployeeToProject")]
        public async Task<IActionResult> AddEmployeeToProject(int id)
        {
            await _mediator.Send(new AddEmployeeToProjectCommand(CurrentUser.IdEmployeeToProject, id));
            _toastService.Success("Added given Employee to Project");
            return RedirectToAction(nameof(Index));
        }

        
    }
}
