using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateRequestValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username-ul este obligatoriu.")
                .MaximumLength(50).WithMessage("Username-ul nu poate depăși 50 de caractere.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Parola este obligatorie.");

            RuleFor(u => u.Role.Id)
                .GreaterThan(0).WithMessage("Rolul este obligatoriu.");

            RuleFor(u => u.Person.Id)
                .GreaterThan(0).WithMessage("Persoana este obligatorie.");
        }
    }
}