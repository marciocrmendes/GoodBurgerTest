using GoodBurger.Api.Domain.Abstractions;

namespace GoodBurger.Api.Domain.Entities;

public class MenuItem : BaseEntity<Guid>
{
    private MenuItem() { }

    public int ItemCategoryId { get; set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string? Description { get; private set; }

    public virtual ItemCategory ItemCategory { get; private set; } = null!;

    public static MenuItem Create(int itemCategoryId,
        string name,
        decimal price,
        string description = "")
    {
        return new MenuItem
        {
            Id = Guid.NewGuid(),
            ItemCategoryId = itemCategoryId,
            Name = name,
            Price = price,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(int categoryId, string name, decimal price, string? description)
    {
        ItemCategoryId = categoryId;
        Name = name;
        Price = price;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
