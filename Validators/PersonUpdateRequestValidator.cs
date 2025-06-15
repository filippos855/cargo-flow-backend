using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class PersonUpdateRequestValidator : AbstractValidator<PersonUpdateRequest>
    {
        public PersonUpdateRequestValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("Id-ul trebuie să fie un număr pozitiv.");

            Include(new PersonCreateRequestValidator());
        }
    }
}
