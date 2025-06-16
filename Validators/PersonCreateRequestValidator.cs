using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class PersonCreateRequestValidator : AbstractValidator<PersonCreateRequest>
    {
        public PersonCreateRequestValidator()
        {
            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("Numele complet este obligatoriu.")
                .Length(3, 100).WithMessage("Numele complet trebuie să aibă între 3 și 100 de caractere.");

            RuleFor(p => p.Cnp)
                .NotEmpty().WithMessage("CNP-ul este obligatoriu.")
                .Matches(@"^\d{13}$").WithMessage("CNP-ul trebuie să conțină exact 13 cifre.");

            RuleFor(p => p.Email)
                .MaximumLength(100).WithMessage("Email-ul nu poate depăși 100 de caractere.")
                .EmailAddress().When(e => !string.IsNullOrWhiteSpace(e.Email))
                .WithMessage("Adresa de email nu este validă.");

            RuleFor(p => p.Phone)
                .MaximumLength(20).WithMessage("Telefonul nu poate depăși 20 de caractere.")
                .Matches(@"^[0-9+\-\s]+$").When(p => !string.IsNullOrWhiteSpace(p.Phone))
                .WithMessage("Numărul de telefon nu este valid.");
        }
    }
}
