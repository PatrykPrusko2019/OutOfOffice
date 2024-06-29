
using MediatR;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.Project.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IProjectRepository _repository;

        public DeleteProjectCommandHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetProjectById(request.Id);

            await _repository.Delete(project);

            return Unit.Value;
        }
    }
}
