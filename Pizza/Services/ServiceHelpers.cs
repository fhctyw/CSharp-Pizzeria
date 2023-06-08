namespace Pizza
{
    public static class ServiceHelpers
    {
        public static void CheckAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException("Address cannot be null or emptry");
            }

            if (address.Length <= 2)
            {
                throw new ArgumentException($"Length of name \"{address}\" cannot be less than or equal 2 symbols");
            }
        }
        public static void CheckCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("Customer cannot be null");
            }

            CheckName(customer.Name);
            CheckAddress(customer.Address);
            CheckMoney(customer.Money);
        }
        public static void CheckIngredient(Ingredient ingredient)
        {
            if (ingredient == null)
            {
                throw new ArgumentNullException("Ingredient cannot be null");
            }

            CheckName(ingredient.Name);

            decimal price = ingredient.Price;
            if (price < IngredientHelpers.MinPrice || price > IngredientHelpers.MaxPrice)
            {
                throw new ArgumentException($"Price {price} cannot be" +
                    $" less than {IngredientHelpers.MinPrice} or great than {IngredientHelpers.MaxPrice}");
            }
        }
        public static void CheckMoney(decimal money)
        {
            if (money <= 0)
            {
                throw new ArgumentException($"Money cannot be less than or equal zero");
            }
        }
        public static void CheckName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name cannot be null or emptry");
            }

            if (name.Length <= 2)
            {
                throw new NameLengthNotEnoughException($"Length of name \"{name}\" cannot be less than or equal 2 symbols");
            }
        }
        public static void CheckPizza(StandardPizza pizza)
        {
            if (pizza == null)
            {
                throw new ArgumentNullException($"Pizza cannot be null");
            }

            CheckName(pizza.Name);
            CheckPrice(pizza.Price);

            if (!IsNeedIngredientsValid(pizza.NeededIngredients))
            {
                throw new ArgumentException($"Pizza ingredients count must be" +
                    $" >= {StandardPizzaHelpers.MinNeededIngredients} and <= {StandardPizzaHelpers.MaxNeededIngredients}");
            }
        }
        public static void CheckPrice(decimal price)
        {
            if (price <= 0)
            {
                throw new WrongPriceException($"Price cannot be less than or equal zero");
            }
        }
        public static bool IsNeedIngredientsValid(Dictionary<string, int> neededIngredients)
        {
            foreach (var pair in neededIngredients)
            {
                if (pair.Value < StandardPizzaHelpers.MinNeededIngredients
                    || pair.Value > StandardPizzaHelpers.MaxNeededIngredients)
                {
                    return false;
                }
            }
            return true;
        }
    }
}