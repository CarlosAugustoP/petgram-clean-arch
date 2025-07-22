using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Abstractions.Reports
{
    public class ReportUserCommandValidator : AbstractValidator<ReportUserCommand>
    {
        public ReportUserCommandValidator()
        {
            RuleFor(x => x.ReporterId).NotEmpty().WithMessage("Reporter ID is required.");
            RuleFor(x => x.ReportedId).NotEmpty().WithMessage("Reported ID is required.");
            RuleFor(x => x.ReasonText).NotEmpty().WithMessage("Reason text is required.")
                .MaximumLength(500).WithMessage("Reason text cannot exceed 500 characters.");
            RuleFor(x => x.ReasonType).IsInEnum().WithMessage("Invalid reason type specified.");
            RuleFor(x => x.PostIds).Must(ids => ids == null || ids.Count <= 10)
                .WithMessage("You can report a maximum of 10 posts.");
            RuleFor(x => x.MomentIds).Must(ids => ids == null || ids.Count <= 10)
                .WithMessage("You can report a maximum of 10 moments.");
        }
    }
}