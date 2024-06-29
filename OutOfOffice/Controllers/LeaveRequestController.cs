using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.ApprovalRequest.Commands.CreateApprovalRequest;
using OutOffOffice.Application.ApprovalRequest.Commands.DeleteApprovalRequest;
using OutOffOffice.Application.ApprovalRequest.Queries.GetApprovalRequestById;
using OutOffOffice.Application.Employee.Commands.EditEmployee;
using OutOffOffice.Application.Employee.Queries.GetAllEmployees;
using OutOffOffice.Application.Employee.Queries.GetEmployeeById;
using OutOffOffice.Application.LeaveRequest;
using OutOffOffice.Application.LeaveRequest.Commands.CreateLeaveRequest;
using OutOffOffice.Application.LeaveRequest.Commands.DeleteLeaveRequest;
using OutOffOffice.Application.LeaveRequest.Commands.EditLeaveRequest;
using OutOffOffice.Application.LeaveRequest.Queries.GetAllLEaveRequests;
using OutOffOffice.Application.LeaveRequest.Queries.GetLeaveRequestById;
using OutOfOffice.Domain.Interfaces;
using X.PagedList;

namespace OutOfOffice.MVC.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IApprovalRequestRepository _approvalRequestRepository;
        private readonly INotyfService _toastService;

        public LeaveRequestController(IMediator mediator, IMapper mapper, IUserContext userContext, IApprovalRequestRepository approvalRequestRepository, INotyfService toastService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userContext = userContext;
            _approvalRequestRepository = approvalRequestRepository;
            _toastService = toastService;
        }

        [Route("Lists/LeaveRequests")]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchLeaveRequestId, int? page)
        {
            int pageSize = 7;
            int pageNumber = (page ?? 1);

            string name = _userContext.GetCurrentUser()?.Email;

            var employees = await _mediator.Send(new GetAllEmployeesQuery());

            IEnumerable<LeaveRequestDto> searchLeaveRequests = new List<LeaveRequestDto>();

            if(CurrentUser.Role == "EMPLOYEE")
            {
                var searchEmployee = employees.FirstOrDefault(e => e.Id == _userContext.GetCurrentUser()?.EmployeeId);
                if (searchEmployee.LeaveRequests != null) searchLeaveRequests = searchEmployee.LeaveRequests;
            }
            else if (CurrentUser.Role == "HR_MANAGER")
            {
                var result = new List<LeaveRequestDto>();
                var givenEmployees = employees.Where(e => e.IdHrManager == _userContext.GetCurrentUser()?.EmployeeId).ToList();
                for (int i = 0; i < givenEmployees.Count(); i++)
                {
                    var searchRequests = givenEmployees.ElementAt(i).LeaveRequests;
                    if (searchLeaveRequests != null) result.AddRange(searchRequests);
                }
                searchLeaveRequests = result;

            } else if (CurrentUser.Role == "PROJECT_MANAGER")
            {
                var projects = employees.FirstOrDefault(p => p.Id == _userContext.GetCurrentUser().EmployeeId).Projects; // project given PROJECT_MANAGER

                var result = new List<LeaveRequestDto>();

                for (int i = 0; i < projects.Count(); i++)
                {
                    var project = projects.ElementAt(i);

                    var searchEmployees = employees.Where(e => e.Subdivision == project.Id.ToString()); // Subdivision == ProjectId

                    for (int j = 0; j < searchEmployees.Count(); j++)
                    {
                        var employee = searchEmployees.ElementAt(j);
                        result.AddRange(employee.LeaveRequests);
                    }
                }
                searchLeaveRequests = result;
            } else
            { //ADMIN
                searchLeaveRequests = await _mediator.Send(new GetAllLEaveRequestsQuery());
            }
            
            if (searchLeaveRequests != null)
            {
                if (searchLeaveRequestId != null)
                {
                    page = 1;
                }
                else
                {
                    searchLeaveRequestId = currentFilter;
                }

                ViewBag.CurrentFilter = searchLeaveRequestId;

                if (!String.IsNullOrEmpty(searchLeaveRequestId))
                {
                    searchLeaveRequests = FindLeaveRequestsByLeaveRequestId(searchLeaveRequestId, searchLeaveRequests);
                }

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "absenceReasone_desc" : "";
                ViewBag.DateSortParm = sortOrder == "StartDate" ? "startDate_desc" : "StartDate";
                ViewBag.DateSortParm = sortOrder == "EndDate" ? "endDate_desc" : "EndDate";
                ViewBag.NameSortParm = sortOrder == "Comment" ? "comment_desc" : "Comment";
                ViewBag.NameSortParm = sortOrder == "Status" ? "status_desc" : "Status";
                ViewBag.NameSortParm = sortOrder == "Id" ? "id_desc" : "Id";
                ViewBag.NameSortParm = sortOrder == "EmployeeId" ? "id_employeeId" : "Id";

                switch (sortOrder)
                {
                    case "projectType_desc":
                        searchLeaveRequests = searchLeaveRequests.OrderByDescending(s => s.AbsenceReason);
                        break;
                    case "StartDate":
                        searchLeaveRequests = searchLeaveRequests.OrderBy(s => s.StartDate);
                        break;
                    case "startDate_desc":
                        searchLeaveRequests = searchLeaveRequests.OrderByDescending(s => s.StartDate);
                        break;
                    case "EndDate":
                        searchLeaveRequests = searchLeaveRequests.OrderBy(s => s.EndDate);
                        break;
                    case "endDate_desc":
                        searchLeaveRequests = searchLeaveRequests.OrderByDescending(s => s.EndDate);
                        break;
                    case "Comment":
                        searchLeaveRequests = searchLeaveRequests.OrderBy(s => s.Comment);
                        break;
                    case "comment_desc":
                        searchLeaveRequests = searchLeaveRequests.OrderByDescending(s => s.Comment);
                        break;
                    case "":
                        searchLeaveRequests = searchLeaveRequests.OrderBy(s => s.Status);
                        break;
                    case "status_desc":
                        searchLeaveRequests = searchLeaveRequests.OrderByDescending(s => s.Status);
                        break;
                    case "Id":
                        searchLeaveRequests = searchLeaveRequests.OrderBy(s => s.Id);
                        break;
                    case "id_desc":
                        searchLeaveRequests = searchLeaveRequests.OrderByDescending(s => s.Id);
                        break;
                    case "EmployeeId":
                        searchLeaveRequests = searchLeaveRequests.OrderBy(s => s.EmployeeId);
                        break;
                    case "employeeId_desc":
                        searchLeaveRequests = searchLeaveRequests.OrderByDescending(s => s.EmployeeId);
                        break;
                    default:
                        searchLeaveRequests = searchLeaveRequests.OrderBy(s => s.AbsenceReason);
                        break;
                }


                return View(searchLeaveRequests.ToPagedList(pageNumber, pageSize));
            }

            return View(searchLeaveRequests.ToPagedList(pageNumber, pageSize));
        }

        private IEnumerable<LeaveRequestDto> FindLeaveRequestsByLeaveRequestId(string searchLeaveRequestId, IEnumerable<LeaveRequestDto> leaveRequests)
        {
            if (!String.IsNullOrEmpty(searchLeaveRequestId))
            {
                var searchedLeaveRequest = leaveRequests.Where(s => searchLeaveRequestId == s.Id.ToString()).ToList();
                return searchedLeaveRequest;
            }
            return leaveRequests;
        }


        
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        public async Task<IActionResult> Create(CreateLeaveRequestCommand employee)
        {

            if (!ModelState.IsValid)
            {
                _toastService.Error("no Leave Request created");
                return View(employee);
            }

            await _mediator.Send(employee);
            _toastService.Success("Added new Leave Request");
            return RedirectToAction(nameof(Index));
        }

        [Route("LeaveRequest/{id}/ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var leaveRequest = await _mediator.Send(new GetLeaveRequestByIdQuery(id));

            if (leaveRequest.Status == "NEW" || leaveRequest.Status == "CANCELLED")
            {
                CreateApprovalRequestCommand createApprovalRequest = new CreateApprovalRequestCommand();
                createApprovalRequest.ApproverEmployeeId = leaveRequest.EmployeeId;
                createApprovalRequest.LeaveRequestId = leaveRequest.Id;
                createApprovalRequest.Status = "NEW";
                createApprovalRequest.Comment = leaveRequest.Comment;

                await _mediator.Send(createApprovalRequest);

                var newApprovalRequest = await _approvalRequestRepository.GetApprovalReguestByLeaveRequestId(leaveRequest.Id);
                leaveRequest.Status = "SUBMITTED";
                // leaveRequest.ApprovalRequestId = newApprovalRequest.Id;
                EditLeaveRequestCommand model = _mapper.Map<EditLeaveRequestCommand>(leaveRequest);

                _toastService.Success("updated given Leave Request (created new Approval Request)");
                await _mediator.Send(model);
            }
            else
            {
                //removed ApprovalRequest
                var removedApprovalRequest = await _approvalRequestRepository.GetApprovalReguestByLeaveRequestId(leaveRequest.Id);
                await _mediator.Send(new DeleteApprovalRequestCommand(removedApprovalRequest.Id));

                //changed status
                leaveRequest.Status = "CANCELLED";
                EditLeaveRequestCommand model = _mapper.Map<EditLeaveRequestCommand>(leaveRequest);

                _toastService.Success("updated given Leave Request (deleted given Approval Request)");
                await _mediator.Send(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("LeaveRequest/{id}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var leaveRequestDto = await _mediator.Send(new GetLeaveRequestByIdQuery(id));

            if (_userContext.GetCurrentUser()?.EmployeeId != leaveRequestDto.EmployeeId)
            {
                _toastService.Error("no Leave Request updated)");
                return RedirectToAction("NoAccess", "Home");
            }

            EditLeaveRequestCommand model = _mapper.Map<EditLeaveRequestCommand>(leaveRequestDto);

            return View(model);
        }

        [HttpPost]
        [Route("LeaveRequest/{id}/Edit")]
        public async Task<IActionResult> Edit(int id, EditLeaveRequestCommand leaveRequest)
        {

            if (!ModelState.IsValid)
            {
                return View(leaveRequest);
            }
            _toastService.Success("updated given Leave Request");
            await _mediator.Send(leaveRequest);
            return RedirectToAction(nameof(Index));
        }

        [Route("LeaveRequest/{id}/Details")]
        public async Task<IActionResult> Details(int id)
        {
            var leaveRequestDto = await _mediator.Send(new GetLeaveRequestByIdQuery(id));
            return View(leaveRequestDto);
        }

        [Route("LeaveRequest/{id}/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var leaveRequestDto = await _mediator.Send(new GetLeaveRequestByIdQuery(id));

            if (leaveRequestDto.Status == "APPROVED_REQUEST")
            {
                //check if correct free days by given employee
                var employee = await _mediator.Send(new GetEmployeeByIdQuery(leaveRequestDto.EmployeeId));
                var days = leaveRequestDto.EndDate.Subtract(leaveRequestDto.StartDate).Days;

                employee.OutOfOfficeBalance += days; // return free days by employee
                EditEmployeeCommand changedFreeDaysGivenEmployee = _mapper.Map<EditEmployeeCommand>(employee);
                await _mediator.Send(changedFreeDaysGivenEmployee);

            }

            var approvalRequestDto = await _mediator.Send(new GetApprovalRequestByIdQuery(leaveRequestDto.Id));
            if (approvalRequestDto != null)
            { 
                await _mediator.Send(new DeleteApprovalRequestCommand(approvalRequestDto.Id));
            }
            await _mediator.Send(new DeleteLeaveRequestCommand(id));

            _toastService.Success("deleted given Leave Request");
            return RedirectToAction(nameof(Index));
        }




    }
}
