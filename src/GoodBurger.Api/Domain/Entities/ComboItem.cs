using GoodBurger.Api.Domain.Abstractions;

namespace GoodBurger.Api.Domain.Entities;

public class ComboItem : BaseEntity<Guid>
{
    private ComboItem() { }

    public Guid ComboId { get; private set; }
    public Guid MenuItemId { get; private set; }

    public virtual Combo Combo { get; private set; } = null!;
    public virtual MenuItem MenuItem { get; private set; } = null!;

    internal static ComboItem Create(Guid comboId, Guid itemId)
        => new()
        {
            ComboId = comboId,
            MenuItemId = itemId
        };
}
