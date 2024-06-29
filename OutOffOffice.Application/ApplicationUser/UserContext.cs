using MediatR;
using Microsoft.AspNetCore.Http;
using OutOfOffice.Domain.Interfaces;
using System.Security.Claims;

namespace OutOffOffice.Application.ApplicationUser
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }

    public class UserContext : IUserContext
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeRepository _employeeRepository;
        
        public UserContext(IHttpContextAccessor contextAccessor, IEmployeeRepository employeeRepository, IMediator mediator)
        {
            _httpContextAccessor = contextAccessor;
            _employeeRepository = employeeRepository;
            _mediator = mediator;
        }

        public CurrentUser? GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) 
            {
                throw new InvalidOperationException("Context user is not present");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;

            int index = email.IndexOf('@');
            string name = email.Substring(0, index);
            var employee = _employeeRepository.GetByName(name).Result;
            
            var position = employee?.Position;
            int employeeId = default;
            if (employee != null) { employeeId = employee.Id;}

            return new CurrentUser(id, name, employeeId, position);
        }
    }
}
