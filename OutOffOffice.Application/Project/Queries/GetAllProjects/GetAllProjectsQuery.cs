
using MediatR;
namespace OutOffOffice.Application.Project.Queries.GetAllProjects
{
    public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
    }
}
