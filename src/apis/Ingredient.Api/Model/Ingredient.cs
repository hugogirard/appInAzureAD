namespace Ingredient.Api.Model;

public class Ingredient
{
    public string Id { get; set; }

    public string Name { get; set; }

    public IngredientType IngredientType { get; set; }

    public Ingredient(string name)
    {
        Id = new Guid().ToString();
        Name = name;
    }
}