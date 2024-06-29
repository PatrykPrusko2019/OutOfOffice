using MediatR;
using Microsoft.AspNetCore.Mvc;
using OutOffOffice.Application.ApplicationUser;
using OutOfOffice.Models;
using System.Diagnostics;

namespace OutOfOffice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;
        private readonly IUserContext _userContext;

        public HomeController(ILogger<HomeController> logger, IMediator mediator, IUserContext userContext)
        {
            _logger = logger;
            _mediator = mediator;
            _userContext = userContext; 
        }

        public async Task<IActionResult> Index()
        {
            _userContext?.GetCurrentUser();
            
            return View();
        }

        public IActionResult NoAccess()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
