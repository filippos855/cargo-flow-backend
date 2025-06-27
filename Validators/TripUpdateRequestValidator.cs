using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class TripUpdateRequestValidator : AbstractValidator<TripUpdateRequest>
    {
        public TripUpdateRequestValidator()
        {
            RuleFor(t => t.Id)
                .GreaterThan(0).WithMessage("Id-ul trebuie să fie pozitiv.");

            Include(new TripCreateRequestValidator());
        }
    }
}
