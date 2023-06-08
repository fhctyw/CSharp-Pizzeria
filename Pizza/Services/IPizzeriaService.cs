namespace Pizza
{
    public interface IPizzeriaService
    {
        void LoadPizzeria(string fileName);
        void SavePizzeria(string fileName);
        Pizzeria GetPizzeria();
        Ingredient GetAvailableIngredient(string name);
        void AddAvailableIngredient(Ingredient ingredient);
        void ChangeAvailableIngredient(string name, Ingredient ingredient);
        void RemoveAvailableIngredient(string name);
        void AddIngredients(string name, int count);
        StandardPizza GetPizza(string name);
        void AddPizza(StandardPizza pizza);
        void ChangePizza(string name, StandardPizza pizza);
        void RemovePizza(string name);
        bool IsPizzasAvailable(string name);
        List<StandardPizza> GetPizzasByIngredients(List<string> ingredientNames);
        Bill SellPizza(string customerName, Order order);
    }
}
