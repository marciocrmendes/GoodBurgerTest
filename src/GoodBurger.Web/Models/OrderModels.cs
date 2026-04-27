namespace GoodBurger.Web.Models;

public class CreateOrderRequest
{
    public Guid SandwichItemId { get; set; }
    public Guid? FriesItemId { get; set; }
    public Guid? DrinkItemId { get; set; }
}

public class UpdateOrderRequest
{
    public Guid SandwichItemId { get; set; }
    public Guid? FriesItemId { get; set; }
    public Guid? DrinkItemId { get; set; }
}

public class OrderResponse
{
    public Guid Id { get; set; }
    public MenuItemModel SandwichItem { get; set; } = new();
    public MenuItemModel? FriesItem { get; set; }
    public MenuItemModel? DrinkItem { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string FormattedSubtotal => Subtotal.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
    public string FormattedDiscount => DiscountAmount.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
    public string FormattedTotal => Total.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
    public string ItemsSummary
    {
        get
        {
            var parts = new List<string> { SandwichItem.Name };
            if (FriesItem is not null) parts.Add(FriesItem.Name);
            if (DrinkItem is not null) parts.Add(DrinkItem.Name);
            return string.Join(", ", parts);
        }
    }
}

public class ApiError
{
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }
}
