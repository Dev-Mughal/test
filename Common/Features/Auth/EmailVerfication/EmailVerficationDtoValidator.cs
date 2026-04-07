using Common.Features.Auth.Login;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Features.Auth.EmailVerfication
{
    public class EmailVerficationDtoValidator : AbstractValidator<EmailVerificationDto>
    {
        public EmailVerficationDtoValidator()
        {
            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("A valid email is required.");
        }

    }
}
