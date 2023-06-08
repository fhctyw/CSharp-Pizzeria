namespace Pizza
{
    [Serializable]
    public class Pizzeria
    {
        //public string Name { get; set; }
        public List<StandardPizza> Menu { get; set; }
        public Dictionary<Ingredient, int> AvailableIngredients { get; set; }
        public List<Bill> Bills { get; set; }

        public Pizzeria(
            //string name,
            List<StandardPizza> menu,
            Dictionary<Ingredient, int> availableIngredients,
            List<Bill> bills)
        {
            //Name = name;
            Menu = menu;
            AvailableIngredients = availableIngredients;
            Bills = bills;
        }
        public Pizzeria()
        {
            Menu = new();
            AvailableIngredients = new();
            Bills = new();
        }
    }
}
