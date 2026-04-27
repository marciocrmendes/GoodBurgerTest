using FluentValidation;

namespace GoodBurger.Api.Features.Orders.UpdateOrder;

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(x => x.MenuItemIds)
            .NotEmpty().WithMessage("O pedido deve conter ao menos um item.")
            .Must(ids => ids.All(id => id != Guid.Empty)).WithMessage("IDs de item inválidos.")
            .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Itens duplicados não são permitidos.");
    }
}
