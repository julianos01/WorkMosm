using FluentValidation;
using WorkMosm.Application.UseCases.LoginUser.Records;

namespace WorkMosm.Application.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).IsValidEmail();
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
