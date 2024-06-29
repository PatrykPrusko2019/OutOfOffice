
using MediatR;

namespace OutOffOffice.Application.Project.Queries.GetProjectById
{
    public class GetProjectByIdQuery : IRequest<ProjectDto>
    {
        public int Id { get; set; }

        public GetProjectByIdQuery(int id)
        {
            Id = id;
        }
    }
}
