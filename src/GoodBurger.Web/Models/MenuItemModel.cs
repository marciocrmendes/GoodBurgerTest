namespace GoodBurger.Web.Models;

public class MenuItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public string FormattedPrice => Price.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
    public string CategoryLabel => Category switch
    {
        "Sanduiche" => "Sanduíche",
        "Batata" => "Batata Frita",
        "Bebida" => "Bebida",
        _ => Category
    };
}
