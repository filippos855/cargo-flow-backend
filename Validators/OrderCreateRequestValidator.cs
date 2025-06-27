using FluentValidation;
using cargo_flow_backend.Models.Requests;

public class OrderCreateRequestValidator : AbstractValidator<OrderCreateRequest>
{
    public OrderCreateRequestValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Adresa este obligatorie.")
            .MaximumLength(200).WithMessage("Adresa nu poate depăși 200 de caractere.");

        RuleFor(x => x.Company)
            .NotNull().WithMessage("Firma este obligatorie.")
            .Must(c => c.Id > 0).WithMessage("Firma este invalidă.");

        RuleFor(x => x.DeliveryPerson)
            .NotNull().WithMessage("Persoana de livrare este obligatorie.")
            .Must(p => p.Id > 0).WithMessage("Persoana de livrare este invalidă.");

        RuleFor(x => x.Status)
            .NotNull().WithMessage("Statusul este obligatoriu.")
            .Must(s => s.Id > 0).WithMessage("Statusul este invalid.");

        When(x => x.Trip != null, () => {
            RuleFor(x => x.Trip!.Id).GreaterThan(0).WithMessage("Cursa este invalidă.");
        });
    }
}
