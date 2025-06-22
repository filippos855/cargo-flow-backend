using FluentValidation;
using cargo_flow_backend.Controllers;

namespace cargo_flow_backend.Validators
{
    public class LoginRequestValidator : AbstractValidator<AuthController.LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username-ul este obligatoriu.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Parola este obligatorie.");
        }
    }
}