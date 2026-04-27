using FluentValidation;

namespace GoodBurger.Api.Features.MenuItems.UpdateMenuItem;

public class UpdateMenuItemValidator : AbstractValidator<UpdateMenuItemRequest>
{
    public UpdateMenuItemValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Categoria inválida.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero.");

        RuleFor(x => x.Description)
            .MaximumLength(200).When(x => x.Description is not null)
            .WithMessage("Descrição deve ter no máximo 200 caracteres.");
    }
}
