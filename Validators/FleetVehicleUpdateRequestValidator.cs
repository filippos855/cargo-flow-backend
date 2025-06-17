using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class FleetVehicleUpdateRequestValidator : AbstractValidator<FleetVehicleUpdateRequest>
    {
        public FleetVehicleUpdateRequestValidator()
        {
            RuleFor(v => v.Id)
                .GreaterThan(0).WithMessage("Id-ul trebuie să fie pozitiv.");

            Include(new FleetVehicleCreateRequestValidator());
        }
    }
}
