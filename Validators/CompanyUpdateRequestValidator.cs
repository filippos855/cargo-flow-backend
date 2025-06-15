using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class CompanyUpdateRequestValidator : AbstractValidator<CompanyUpdateRequest>
    {
        public CompanyUpdateRequestValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Id-ul trebuie să fie pozitiv.");

            Include(new CompanyCreateRequestValidator());
        }
    }
}
