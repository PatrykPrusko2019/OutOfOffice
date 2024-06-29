using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutOffOffice.Application.ApplicationUser;
using OutOffOffice.Application.ApprovalRequest;
using OutOffOffice.Application.ApprovalRequest.Commands.EditAprovalRequest;
using OutOffOffice.Application.ApprovalRequest.Queries.GetAllApprovalRequests;
using OutOffOffice.Application.ApprovalRequest.Queries.GetApprovalRequestById;
using OutOffOffice.Application.Employee.Commands.EditEmployee;
using OutOffOffice.Application.Employee;
using OutOffOffice.Application.Employee.Queries.GetEmployeeById;
using OutOffOffice.Application.LeaveRequest.Commands.EditLeaveRequest;
using OutOffOffice.Application.LeaveRequest.Queries.GetLeaveRequestById;
using X.PagedList;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OutOfOffice.MVC.Controllers
{
    [Authorize]
    public class ApprovalRequestController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly INotyfService _toastService;
        public static string CurrentStatus { get; set; }

        public ApprovalRequestController(IMediator mediator, IMapper mapper, IUserContext userContext, INotyfService toastService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userContext = userContext;
            _toastService = toastService;
        }

        [Route("Lists/ApprovalRequests")]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchApprovalRequestId, int? page)
        {
            int pageSize = 7;
            int pageNumber = (page ?? 1);

            
            var employeeId = _userContext.GetCurrentUser()?.EmployeeId;

            var approvalRequests = await _mediator.Send(new GetAllApprovalRequestsQuery());

            var givenRequests = approvalRequests.Where(r => r.ApproverEmployeeId == employeeId);

            if (givenRequests != null)
            {
                if (searchApprovalRequestId != null)
                {
                    page = 1;
                }
                else
                {
                    searchApprovalRequestId = currentFilter;
                }

                ViewBag.CurrentFilter = searchApprovalRequestId;

                if (!String.IsNullOrEmpty(searchApprovalRequestId))
                {
                    approvalRequests = FindLeaveRequestsByLeaveRequestId(searchApprovalRequestId, approvalRequests);
                }

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "";
                ViewBag.NameSortParm = sortOrder == "Id" ? "id_desc" : "Id";
                ViewBag.NameSortParm = sortOrder == "Comment" ? "comment_desc" : "Comment";
                ViewBag.NameSortParm = sortOrder == "ApproverEmployeeId" ? "approverEmployeeId_desc" : "ApproverEmployeeId";
                ViewBag.NameSortParm = sortOrder == "LeaveRequestId" ? "leaveRequestId_desc" : "LeaveRequestId";

                switch (sortOrder)
                {
                    case "status_desc":
                        approvalRequests = approvalRequests.OrderByDescending(s => s.Status);
                        break;
                    case "Id":
                        approvalRequests = approvalRequests.OrderBy(s => s.LeaveRequestId);
                        break;
                    case "id_desc":
                        approvalRequests = approvalRequests.OrderByDescending(s => s.LeaveRequestId);
                        break;
                    case "Comment":
                        approvalRequests = approvalRequests.OrderBy(s => s.Comment);
                        break;
                    case "comment_desc":
                        approvalRequests = approvalRequests.OrderByDescending(s => s.Comment);
                        break;
                    case "ApproverEmployeeId":
                        approvalRequests = approvalRequests.OrderBy(s => s.ApproverEmployeeId);
                        break;
                    case "approverEmployeeId_desc":
                        approvalRequests = approvalRequests.OrderByDescending(s => s.ApproverEmployeeId);
                        break;
                    case "LeaveRequestId":
                        approvalRequests = approvalRequests.OrderBy(s => s.LeaveRequestId);
                        break;
                    case "leaveRequestId_desc":
                        approvalRequests = approvalRequests.OrderByDescending(s => s.LeaveRequestId);
                        break;
                    default:
                        approvalRequests = approvalRequests.OrderBy(s => s.Status);
                        break;
                }


                return View(approvalRequests.ToPagedList(pageNumber, pageSize));
            }

            return View(approvalRequests.ToPagedList(pageNumber, pageSize));
        }

        private IEnumerable<ApprovalRequestDto> FindLeaveRequestsByLeaveRequestId(string searchapprovalRequestId, IEnumerable<ApprovalRequestDto> approvalRequests)
        {
            if (!String.IsNullOrEmpty(searchapprovalRequestId))
            {
                var searchedApprovalRequest = approvalRequests.Where(s => searchapprovalRequestId == s.Id.ToString()).ToList();
                return searchedApprovalRequest;
            }
            return approvalRequests;
        }

        [Route("ApprovalRequest/{id}/Details")]
        public async Task<IActionResult> Details(int id)
        {
            var approvalRequestDto = await _mediator.Send(new GetApprovalRequestByIdQuery(id));
            return View(approvalRequestDto);
        }

        [Route("ApprovalRequest/{id}/ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var approvalRequestDto = await _mediator.Send(new GetApprovalRequestByIdQuery(id));

            CurrentStatus = approvalRequestDto.Status;

            EditApprovalRequestCommand model = _mapper.Map<EditApprovalRequestCommand>(approvalRequestDto);

            return View(model);
        }

        [HttpPost]
        [Route("ApprovalRequest/{id}/ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int id, EditApprovalRequestCommand approvalRequest)
        {

            if (!ModelState.IsValid)
            {
                return View(approvalRequest);
            }

            if (approvalRequest.Status == "APPROVED_REQUEST" && CurrentStatus == "REJECTED_REQUEST")
            {
                //check if correct free days by given employee
                var employee = await _mediator.Send(new GetEmployeeByIdQuery(approvalRequest.ApproverEmployeeId));
                var leaveRequestDto = await _mediator.Send(new GetLeaveRequestByIdQuery(approvalRequest.LeaveRequestId));
                var days = leaveRequestDto.EndDate.Subtract(leaveRequestDto.StartDate).Days;

                if (employee.OutOfOfficeBalance >= days)
                {
                    employee.OutOfOfficeBalance -= days;
                    EditEmployeeCommand changedFreeDaysGivenEmployee = _mapper.Map<EditEmployeeCommand>(employee);
                    await _mediator.Send(changedFreeDaysGivenEmployee);
                }
                else
                {
                    _toastService.Error("no vacation days for the employee");
                    approvalRequest.Status = "REJECTED_REQUEST";
                }

            } else if (approvalRequest.Status == "REJECTED_REQUEST" && CurrentStatus == "APPROVED_REQUEST")
            {
                //check if correct free days by given employee
                var employee = await _mediator.Send(new GetEmployeeByIdQuery(approvalRequest.ApproverEmployeeId));
                var leaveRequestDto = await _mediator.Send(new GetLeaveRequestByIdQuery(approvalRequest.LeaveRequestId));
                var days = leaveRequestDto.EndDate.Subtract(leaveRequestDto.StartDate).Days;

                employee.OutOfOfficeBalance += days; // return free days by employee
                EditEmployeeCommand changedFreeDaysGivenEmployee = _mapper.Map<EditEmployeeCommand>(employee);
                await _mediator.Send(changedFreeDaysGivenEmployee);
            }

            await _mediator.Send(approvalRequest);

            var leaveRequest = await _mediator.Send(new GetLeaveRequestByIdQuery(approvalRequest.LeaveRequestId));
            EditLeaveRequestCommand model = _mapper.Map<EditLeaveRequestCommand>(leaveRequest);
            model.Status = approvalRequest.Status;
            model.Comment = approvalRequest.Comment;

            await _mediator.Send(model);
            _toastService.Success("Updated given Approval Request");
            return RedirectToAction(nameof(Index));
        }

    }
}
