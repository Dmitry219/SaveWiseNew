using FluentValidation;
using SaveWiseNew.DTO;

namespace SaveWiseNew.Validation
{
    public class UserIdRequestValidator : AbstractValidator<UserIdRequest>
    {
        public UserIdRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id должен быть больше 0");
        }
    }
}
