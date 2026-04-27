using FluentValidation;

namespace GoodBurger.Api.Features.Combos.CreateCombo;

public class CreateComboValidator : AbstractValidator<CreateComboRequest>
{
    public CreateComboValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Descrição deve ter no máximo 200 caracteres");

        RuleFor(x => x.DiscountPercentage)
            .InclusiveBetween(0m, 100m).WithMessage("Percentual de desconto deve estar entre 0 e 100");

        RuleFor(x => x.MenuItemIds)
            .NotEmpty().WithMessage("Pelo menos um item é necessário")
            .Must(ids => ids.All(id => id != Guid.Empty)).WithMessage("IDs de item inválidos")
            .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Itens duplicados não são permitidos");
    }
}
