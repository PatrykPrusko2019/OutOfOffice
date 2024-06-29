using MediatR;

namespace OutOffOffice.Application.Employee.Queries.GetAllEmployees
{
    public class GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
    {

    }
}
