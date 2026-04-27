using FluentValidation;

namespace GoodBurger.Api.Features.Orders.CreateOrder;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.MenuItemIds)
            .NotEmpty().WithMessage("O pedido deve conter ao menos um item.")
            .Must(ids => ids.All(id => id != Guid.Empty)).WithMessage("IDs de item inválidos.")
            .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Itens duplicados não são permitidos.");
    }
}
