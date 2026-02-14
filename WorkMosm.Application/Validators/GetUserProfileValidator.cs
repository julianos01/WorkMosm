using FluentValidation;
using WorkMosm.Application.UseCases.GetUserProfile;

namespace WorkMosm.Application.Validators
{
    public class GetUserProfileValidator : AbstractValidator<GetUSerProfileRequest>
    {
        public GetUserProfileValidator()
        {
            RuleFor(x => x.Email).IsValidEmail();
        }
    }
}
