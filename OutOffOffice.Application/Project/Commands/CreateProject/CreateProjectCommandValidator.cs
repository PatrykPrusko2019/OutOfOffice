
using FluentValidation;
using OutOffOffice.Application.Employee.Commands.CreateEmployee;

namespace OutOffOffice.Application.Project.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(e => e.ProjectType).NotEmpty().WithMessage("Please write a reason");
            RuleFor(e => e.Comment).NotEmpty().WithMessage("Please write a something");

            RuleFor(e => e.StartDate)
                .NotEmpty().WithMessage("Please enter: Correct date")
                .Custom((value, context) =>
                {
                    if (value.Date <= DateTime.Now)
                    {
                        context.AddFailure($"date is not current, you can choose the date from : {DateTime.Now}");
                    }

                });

            RuleFor(e => e.EndDate)
                .NotEmpty().WithMessage("Please enter: Correct Date")
                .Custom((value, context) =>
                {
                    if (value <= DateTime.Now)
                    {
                        context.AddFailure($"date is not current, you can choose the date from : {DateTime.Now}");
                    }

                });
        }
    }
}
