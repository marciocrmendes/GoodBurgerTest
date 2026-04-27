using GoodBurger.Api.Domain.Abstractions;

namespace GoodBurger.Api.Domain.Entities;

public class Combo : BaseEntity<Guid>
{
    private Combo() { }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal DiscountPercentage { get; private set; }

    private readonly List<ComboItem> _items = [];
    public virtual IReadOnlyCollection<ComboItem> Items => _items.AsReadOnly();

    public static Combo Create(string name,
        decimal discountPercentage,
        string description = "",
        IEnumerable<Guid> menuItemIds = null!)
    {
        var combo = new Combo
        {
            Id = Guid.NewGuid(),
            Name = name,
            DiscountPercentage = discountPercentage,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        foreach (var itemId in menuItemIds.Distinct())
            combo._items.Add(ComboItem.Create(combo.Id, itemId));

        return combo;
    }
}