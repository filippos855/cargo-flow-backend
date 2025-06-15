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
                .Must(cnp => cnp.Trim().Length == 13 && cnp.All(char.IsDigit))
                .WithMessage("CNP-ul trebuie să conțină exact 13 cifre.");

            RuleFor(p => p.Email)
                .Cascade(CascadeMode.Stop)
                .Must(email => string.IsNullOrWhiteSpace(email) || IsValidEmail(email))
                .WithMessage("Adresa de email nu este validă.");

            RuleFor(p => p.Phone)
                .Cascade(CascadeMode.Stop)
                .Must(phone => string.IsNullOrWhiteSpace(phone) || IsValidPhone(phone))
                .WithMessage("Numărul de telefon nu este valid.");
        }

        private bool IsValidEmail(string? email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email!);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string? phone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phone!, @"^[0-9+\-\s]+$");
        }
    }
}
