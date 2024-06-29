
using FluentValidation;
using OutOffOffice.Application.Employee.Commands.EditEmployee;

namespace OutOffOffice.Application.ApprovalRequest.Commands.EditAprovalRequest
{
    public class EditApprovalRequestCommandValidator : AbstractValidator<EditApprovalRequestCommand>
    {
        public EditApprovalRequestCommandValidator()
        {
            RuleFor(e => e.Status)
                .Custom((value, context) =>
                {
                    switch (value.ToUpper())
                    {
                        case "APPROVED_REQUEST":
                            break;
                        case "REJECTED_REQUEST":
                            break;
                        default:
                            context.AddFailure($"{value} is not value: APPROVED_REQUEST or REJECTED_REQUEST");
                            break;

                    }

                });

            RuleFor(e => e.Comment)
                .NotEmpty().WithMessage("Please write the reason why");
               
        }
    }
}
