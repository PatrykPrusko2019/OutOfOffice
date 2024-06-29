
using FluentValidation;
using OutOfOffice.Domain.Interfaces;

namespace OutOffOffice.Application.Employee.Commands.CreateEmployee
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator(IEmployeeRepository repository)
        {
            RuleFor(e => e.FullName)
                .NotEmpty()
                .MinimumLength(10).WithMessage("FullName should have atleast 10 characters")
                .MaximumLength(25).WithMessage("FullName should have max 25 characters")
                .Custom((value, context) =>
                {
                    var existingEmployee = repository.GetByName(value).Result;
                    if (existingEmployee != null)
                    {
                        context.AddFailure($"{value} is not unigue Fullname for Employee");
                    }
                });

            RuleFor(e => e.Status)
                .NotEmpty().WithMessage("please enter: Active or Inactive")
                .Custom((value, context) =>
                {
                    var result = "Active".ToLower().Equals(value.ToLower()) || "Inactive".ToLower().Equals(value.ToLower());
                    if (result == false)
                    {
                        context.AddFailure($"{value} is not value: Active or Inactive");
                    }
                });

            RuleFor(e => e.Position)
                .NotEmpty().WithMessage("Please enter: EMPLOYEE or HR_MANAGER or PROJECT_MANAGER or ADMIN")
                .Custom((value, context) =>
                {
                    switch (value?.ToUpper())
                    {
                        case "EMPLOYEE":
                            break;
                        case "HR_MANAGER":
                            break;
                        case "PROJECT_MANAGER":
                            break;
                        case "ADMIN":
                            break;
                        default:
                            context.AddFailure($"{value} is not value: EMPLOYEE or HR_MANAGER or PROJECT_MANAGER or ADMIN");
                            break;

                    }

                });

            RuleFor(e => e.OutOfOfficeBalance).NotEmpty().WithMessage("Please enter: value > 0")
                .GreaterThan(0).WithErrorCode("Please enter: value > 0");
        }

    }
}
