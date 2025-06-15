using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class CompanyCreateRequestValidator : AbstractValidator<CompanyCreateRequest>
    {
        public CompanyCreateRequestValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Numele firmei este obligatoriu.")
                .MaximumLength(100).WithMessage("Numele nu poate depăși 100 de caractere.");

            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("Codul firmei este obligatoriu.")
                .MaximumLength(20).WithMessage("Codul nu poate depăși 20 de caractere.");

            RuleFor(c => c.Cui)
                .Cascade(CascadeMode.Stop)
                .Must(cui => string.IsNullOrWhiteSpace(cui) || System.Text.RegularExpressions.Regex.IsMatch(cui, @"^\d{8,10}$"))
                .WithMessage("CUI-ul trebuie să conțină între 8 și 10 cifre.");

            RuleFor(c => c.Address)
                .MaximumLength(200).WithMessage("Adresa nu poate depăși 200 de caractere.");

            RuleFor(c => c.ContactPersonId)
                .GreaterThan(0).WithMessage("Trebuie să selectezi o persoană de contact.");
        }
    }
}
