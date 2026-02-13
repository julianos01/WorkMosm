using Application.UseCases.RegisterUser;
using FluentValidation;

namespace Application.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("The provided {PropertyName} address is not valid.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password required")
                                    .MinimumLength(6).WithMessage("Password is too short (minimum 6).")
                                    .MaximumLength(50).WithMessage("Password cannot exceed 50 characters.");
        }
    }
}
