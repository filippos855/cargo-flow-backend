using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class FleetVehicleCreateRequestValidator : AbstractValidator<FleetVehicleCreateRequest>
    {
        public FleetVehicleCreateRequestValidator()
        {
            RuleFor(v => v.Identifier)
                .NotEmpty().WithMessage("Identificatorul este obligatoriu.")
                .MaximumLength(50).WithMessage("Identificatorul nu poate depăși 50 de caractere.");

            RuleFor(v => v.Type.Id)
                .GreaterThan(0).WithMessage("Tipul vehiculului este obligatoriu.");

            RuleFor(v => v.ItpExpiration)
                .GreaterThan(DateTime.MinValue).WithMessage("Data expirării ITP este obligatorie.");

            RuleFor(v => v.RcaExpiration)
                .GreaterThan(DateTime.MinValue).WithMessage("Data expirării RCA este obligatorie.");
        }
    }
}
