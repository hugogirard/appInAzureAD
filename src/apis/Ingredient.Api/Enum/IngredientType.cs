namespace Ingredient.Api.Enum
{
    public class IngredientType
    {
        private IngredientType(string value) { Value = value; }

        public string Value { get; private set; }

        public static IngredientType Veggie { get { return new IngredientType("Veggie"); } }

        public static IngredientType Meat { get { return new IngredientType("Meat"); } }

        public static IngredientType Cheese { get { return new IngredientType("Cheese"); } }

    }
}
