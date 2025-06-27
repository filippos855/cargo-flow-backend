using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class TripCreateRequestValidator : AbstractValidator<TripCreateRequest>
    {
        public TripCreateRequestValidator()
        {
            RuleFor(t => t.Number)
                .NotEmpty().WithMessage("Numărul cursei este obligatoriu.")
                .MaximumLength(50).WithMessage("Numărul nu poate depăși 50 de caractere.");

            RuleFor(t => t.StartDate)
                .GreaterThan(DateTime.MinValue).WithMessage("Data de început este obligatorie.");

            RuleFor(t => t.Status)
                .NotNull().WithMessage("Statusul este obligatoriu.")
                .Must(s => s.Id > 0).WithMessage("Status invalid.");

            RuleFor(t => t.TransportCompany)
                .NotNull().WithMessage("Firma transport este obligatorie.")
                .Must(c => c.Id > 0).WithMessage("Firma transport invalidă.");

            When(t => t.Driver != null, () =>
            {
                RuleFor(t => t.Driver!.Id)
                    .GreaterThan(0).WithMessage("Șofer invalid.");
            });

            When(t => t.TractorUnit != null, () =>
            {
                RuleFor(t => t.TractorUnit!.Id)
                    .GreaterThan(0).WithMessage("Tractor invalid.");
            });

            When(t => t.Trailer != null, () =>
            {
                RuleFor(t => t.Trailer!.Id)
                    .GreaterThan(0).WithMessage("Remorcă invalidă.");
            });

            When(t => t.Orders != null && t.Orders.Count > 0, () =>
            {
                RuleForEach(t => t.Orders!)
                    .Must(o => o.Id > 0).WithMessage("Comandă invalidă în listă.");
            });
        }
    }
}