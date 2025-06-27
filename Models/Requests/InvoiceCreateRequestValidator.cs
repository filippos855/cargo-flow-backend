using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class InvoiceCreateRequestValidator : AbstractValidator<InvoiceCreateRequest>
    {
        public InvoiceCreateRequestValidator()
        {
            RuleFor(i => i.Number)
                .NotEmpty().WithMessage("Numărul este obligatoriu.")
                .MaximumLength(50);

            RuleFor(i => i.InvoiceType.Id)
                .GreaterThan(0).WithMessage("Tipul facturii este obligatoriu.");

            RuleFor(i => i.Status.Id)
                .GreaterThan(0).WithMessage("Statusul este obligatoriu.");

            RuleFor(i => i.Company.Id)
                .GreaterThan(0).WithMessage("Firma este obligatorie.");

            RuleFor(i => i.Amount)
                .GreaterThan(0).WithMessage("Valoarea trebuie să fie pozitivă.");

            RuleFor(i => i.Currency)
                .NotEmpty().WithMessage("Moneda este obligatorie.")
                .MaximumLength(10);

            RuleFor(i => i.IssueDate)
                .GreaterThan(DateTime.MinValue).WithMessage("Data emiterii este obligatorie.");

            RuleFor(i => i.DueDate)
                .GreaterThanOrEqualTo(i => i.IssueDate).WithMessage("Scadența nu poate fi înainte de emitere.");

            RuleFor(i => new { i.InvoiceType, i.Order, i.Trip })
                .Custom((x, context) =>
                {
                    var typeName = x.InvoiceType?.Name?.ToLowerInvariant();
                    if (typeName == "emisă")
                    {
                        if (x.Order == null)
                            context.AddFailure("Factura emisă trebuie să fie asociată unei comenzi.");
                        if (x.Trip != null)
                            context.AddFailure("Factura emisă nu poate avea și cursă asociată.");
                    }
                    else if (typeName == "primită")
                    {
                        if (x.Trip == null)
                            context.AddFailure("Factura primită trebuie să fie asociată unei curse.");
                        if (x.Order != null)
                            context.AddFailure("Factura primită nu poate avea și comandă asociată.");
                    }
                });
        }
    }
}
