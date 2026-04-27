using GoodBurger.Api.Domain.Abstractions;

namespace GoodBurger.Api.Domain.Entities;

public class ItemCategory : BaseEntity<int>
{
    private ItemCategory() { }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool Active { get; set; }

    public static ItemCategory Create(string name, string description = "")
    {
        return new ItemCategory
        {
            Name = name.Trim(),
            Description = description.Trim(),
            Active = true
        };
    }
}
