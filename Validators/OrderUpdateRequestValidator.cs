using FluentValidation;
using cargo_flow_backend.Models.Requests;

public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
{
    public OrderUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id-ul comenzii trebuie să fie pozitiv.");

        Include(new OrderCreateRequestValidator());
    }
}
