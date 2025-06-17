using FluentValidation;
using cargo_flow_backend.Models.Requests;

namespace cargo_flow_backend.Validators
{
    public class DictionaryItemQueryValidator : AbstractValidator<DictionaryItemQuery>
    {
        public DictionaryItemQueryValidator()
        {
            RuleFor(x => x.DictionaryName)
                .NotEmpty().WithMessage("Numele dicționarului este obligatoriu.")
                .MaximumLength(100).WithMessage("Numele dicționarului nu poate depăși 100 de caractere.");
        }
    }
}
