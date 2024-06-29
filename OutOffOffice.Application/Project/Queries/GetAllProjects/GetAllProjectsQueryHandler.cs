
using MediatR;
using OutOfOffice.Domain.Interfaces;
using AutoMapper;

namespace OutOffOffice.Application.Project.Queries.GetAllProjects
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetAllProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAll();
            var projectsDtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);

            return projectsDtos;
        }
    }
}
