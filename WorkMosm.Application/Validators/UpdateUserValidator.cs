using FluentValidation;
using WorkMosm.Application.UseCases.UpdateUser;

namespace WorkMosm.Application.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required.");
            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email!).IsValidEmail();
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Password is too short (minimum 6).")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 characters.");
            });
        }
    }
}
