using FluentValidation;
using WorkMosm.Application.UseCases.DeactivateUser;

namespace WorkMosm.Application.Validators
{
    public class DeactivateUserValidator : AbstractValidator<DeactivateUserRequest>
    {
        public DeactivateUserValidator()
        {
            RuleFor(x => x.Email).IsValidEmail();
        }
    }
}
