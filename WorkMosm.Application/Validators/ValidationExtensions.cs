using FluentValidation;

namespace WorkMosm.Application.Validators
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email is too long.");
        }
    }
}
