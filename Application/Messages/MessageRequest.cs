using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Application.Messages
{
    public record MessageCommand(Guid SenderId, Guid ReceiverId, string Content) : IRequest<MessageDto>;
    public class MessageRequestValidator : AbstractValidator<MessageCommand>
    {
        public MessageRequestValidator()
        {
            RuleFor(x => x.SenderId).NotEmpty().WithMessage("SenderId is required.");
            RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("ReceiverId is required.");
            RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(2000).WithMessage("Content must not exceed 2000 characters.");
        }
    }
}