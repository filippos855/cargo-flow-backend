using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class InvoiceUpdateRequestValidator : AbstractValidator<InvoiceUpdateRequest>
    {
        public InvoiceUpdateRequestValidator()
        {
            RuleFor(i => i.Id)
                .GreaterThan(0).WithMessage("Id-ul trebuie să fie pozitiv.");

            Include(new InvoiceCreateRequestValidator());
        }
    }
}
