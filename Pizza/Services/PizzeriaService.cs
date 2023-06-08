namespace Pizza
{
    public class PizzeriaService : IPizzeriaService
    {
        protected ISerializer serializer;
        protected ICustomerService customerService;
        protected Pizzeria pizzeria = new Pizzeria();

        public PizzeriaService(ISerializer serializer, ICustomerService customerService)
        {
            this.serializer = serializer;
            this.customerService = customerService;
        }

        public void LoadPizzeria(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("File name cannot be null");
            }
            pizzeria = serializer.Deserialize<Pizzeria>(RepositoryHelpers.GetFilePath(fileName));
        }
        public void SavePizzeria(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("File name cannot be null");
            }
            if (!Directory.Exists(RepositoryHelpers.FolderName))
            {
                Directory.CreateDirectory(RepositoryHelpers.FolderName);
            }
            serializer.Serialize(RepositoryHelpers.GetFilePath(fileName), pizzeria);
        }
        public Pizzeria GetPizzeria()
        {
            return pizzeria;
        }
        public Ingredient GetAvailableIngredient(string name)
        {
            Ingredient? foundIngredient = pizzeria.AvailableIngredients.FirstOrDefault(i => i.Key.Name == name).Key;
            if (foundIngredient == default)
            {
                throw new IngredientNotFoundException($"Cannot find ingredient with \"{name}\" name");
            }
            return foundIngredient;
        }
        public void AddAvailableIngredient(Ingredient ingredient)
        {
            ServiceHelpers.CheckIngredient(ingredient);

            if (pizzeria.AvailableIngredients.ContainsKey(ingredient))
            {
                throw new IngredientExistException($"Ingredient \"{ingredient.Name}\" already exists");
            }
            pizzeria.AvailableIngredients.Add(ingredient, 0);
        }
        public void AddIngredients(string name, int count)
        {
            if (count <= 0)
            {
                throw new IngredientCountLessEqualZeroException($"Ingredient count less or equal zero {count}");
            }
            Ingredient? foundIngredient = pizzeria.AvailableIngredients.FirstOrDefault(i => i.Key.Name == name).Key;
            if (foundIngredient == default)
            {
                throw new IngredientNotFoundException($"Cannot find ingredient with \"{name}\" name");
            }
            pizzeria.AvailableIngredients[foundIngredient] += count;
        }
        public void ChangeAvailableIngredient(string name, Ingredient ingredient)
        {
            ServiceHelpers.CheckIngredient(ingredient);

            Ingredient? foundIngredient = pizzeria.AvailableIngredients.FirstOrDefault(i => i.Key.Name == name).Key;
            if (foundIngredient == default)
            {
                throw new IngredientNotFoundException($"Cannot find ingredient with \"{name}\" name");
            }
            foundIngredient.Price = ingredient.Price;
        }
        public void RemoveAvailableIngredient(string name)
        {
            var foundPair = pizzeria.AvailableIngredients.FirstOrDefault(i => i.Key.Name == name);
            if (foundPair.Key == default)
            {
                throw new IngredientNotFoundException($"Cannot find ingredient with \"{name}\" name");
            }
            pizzeria.AvailableIngredients.Remove(foundPair.Key);
        }

        public StandardPizza GetPizza(string name)
        {
            StandardPizza? foundPizza = pizzeria.Menu.FirstOrDefault(p => p.Name == name);
            if (foundPizza == default)
            {
                throw new PizzaNotFoundException($"Cannot find pizza with \"{name}\" name");
            }
            return foundPizza;
        }
        public void AddPizza(StandardPizza pizza)
        {
            ServiceHelpers.CheckPizza(pizza);

            StandardPizza? foundPizza = pizzeria.Menu.FirstOrDefault(p => p.Name == pizza.Name);
            if (foundPizza != default)
            {
                throw new PizzaExistsException($"Pizza with \"{pizza.Name}\" already exists");
            }

            foreach (var neededIngredientPair in pizza.NeededIngredients)
            {
                var availableIngredientPair = pizzeria.AvailableIngredients.FirstOrDefault(i => i.Key.Name == neededIngredientPair.Key);
                if (availableIngredientPair.Key == default)
                {
                    throw new ArgumentException($"Cannot find ingredient with \"{neededIngredientPair.Key}\" name");
                }
            }

            pizzeria.Menu.Add(pizza);
        }

        public void ChangePizza(string name, StandardPizza pizza)
        {
            ServiceHelpers.CheckPizza(pizza);

            StandardPizza? foundPizza = pizzeria.Menu.FirstOrDefault(p => p.Name == name);
            if (foundPizza == default)
            {
                throw new PizzaNotFoundException($"Cannot find pizza with \"{name}\" name");
            }
            foundPizza.Price = pizza.Price;
            foundPizza.NeededIngredients = pizza.NeededIngredients;
        }

        public void RemovePizza(string name)
        {
            StandardPizza? foundPizza = pizzeria.Menu.FirstOrDefault(p => p.Name == name);
            if (foundPizza == default)
            {
                throw new PizzaNotFoundException($"Cannot find pizza with \"{name}\" name");
            }
            pizzeria.Menu = pizzeria.Menu.Where(c => c.Name != name).ToList();
        }

        public bool IsPizzasAvailable(string name)
        {
            StandardPizza? foundPizza = pizzeria.Menu.FirstOrDefault(p => p.Name == name);
            if (foundPizza == default)
            {
                throw new PizzaNotFoundException(name);
            }

            foreach (var neededIngredientPair in foundPizza.NeededIngredients)
            {
                var availableIngredientPair = pizzeria.AvailableIngredients.FirstOrDefault(i => i.Key.Name == neededIngredientPair.Key);
                if (availableIngredientPair.Key == default)
                {
                    return false;
                }
                if (availableIngredientPair.Value < neededIngredientPair.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public List<StandardPizza> GetPizzasByIngredients(List<string> ingredientNames)
        {
            ingredientNames.ForEach(i => GetAvailableIngredient(i));
            return pizzeria.Menu.Where(p =>
            {

                var b = ingredientNames.All(i => p.NeededIngredients.ContainsKey(i));
                return ingredientNames.All(i => p.NeededIngredients.ContainsKey(i));
            }
            ).ToList();
        }

        public Bill SellPizza(string customerName, Order order)
        {
            Customer customer = customerService.GetCustomer(customerName);
            decimal totalPrice = 0;
            List<StandardPizza> pizzas = new();

            if (order == null)
            {
                throw new ArgumentNullException("Order cannot be null");
            }

            Dictionary<Ingredient, int> availableIngredients = pizzeria.AvailableIngredients.ToDictionary(
                    entry =>
                    new Ingredient()
                    {
                        Name = entry.Key.Name,
                        Price = entry.Key.Price
                    },
                    entry => entry.Value
                );


            foreach (var orderPizzaPair in order.OrderedPizzas)
            {
                StandardPizza? foundPizza = GetPizza(orderPizzaPair.Key);

                foreach (var neededIngredientPair in foundPizza.NeededIngredients)
                {
                    Ingredient neededIngredient = GetAvailableIngredient(neededIngredientPair.Key);

                    int totalNeededIngredients = neededIngredientPair.Value * orderPizzaPair.Value;
                    int foundAvailableIngredient = availableIngredients[neededIngredient];

                    if (foundAvailableIngredient < totalNeededIngredients)
                    {
                        throw new IngredientNotEnoughException($"Not enough ingredients, needed {totalNeededIngredients}");
                    }

                    availableIngredients[neededIngredient] -= totalNeededIngredients;
                }
                pizzas.Add(foundPizza);
                totalPrice += foundPizza.Price * orderPizzaPair.Value;
            }
            foreach (var customPizzaPair in order.CustomPizzas)
            {
                var customPizza = customPizzaPair.Key;

                if (GetPizza(customPizza.Name) != customPizza)
                {
                    throw new ArgumentException($"Invalid custom pizza with \"{customPizza.Name}\" name");
                }

                foreach (var neededIngredientPair in customPizza.NeededIngredients)
                {
                    Ingredient neededIngredient = GetAvailableIngredient(neededIngredientPair.Key);

                    int totalNeededIngredients = neededIngredientPair.Value * customPizzaPair.Value;
                    int foundAvailableIngredient = availableIngredients[neededIngredient];

                    if (foundAvailableIngredient < totalNeededIngredients)
                    {
                        throw new ArgumentException($"Not enough additional ingredients, needed {totalNeededIngredients}");
                    }

                    availableIngredients[neededIngredient] -= totalNeededIngredients;
                }

                foreach (var additionalIngredientPair in customPizza.AdditionalIngredients)
                {
                    Ingredient neededIngredient = GetAvailableIngredient(additionalIngredientPair.Key);

                    int totalNeededIngredients = additionalIngredientPair.Value * customPizzaPair.Value;
                    int foundAvailableIngredient = availableIngredients[neededIngredient];

                    if (foundAvailableIngredient < totalNeededIngredients)
                    {
                        throw new ArgumentException($"Not enough additional ingredients, needed {totalNeededIngredients}");
                    }

                    availableIngredients[neededIngredient] -= totalNeededIngredients;
                }

                pizzas.Add(customPizza);
                totalPrice += (customPizza.Price + pizzeria.AvailableIngredients.Join(customPizza.AdditionalIngredients, i => i.Key.Name, i => i.Key, (i1, _) => i1.Key.Price).Sum()) * customPizzaPair.Value;
            }

            pizzeria.AvailableIngredients = availableIngredients;

            if (customer.Money < totalPrice)
            {
                throw new MoneyNotEnoughException($"Not enough money, total price: {totalPrice}, but money: {customer.Money}");
            }
            customer.Money -= totalPrice;

            var bill = new Bill()
            {
                Customer = customer,
                Time = DateTime.Now,
                TotalPrice = totalPrice,
                Pizzas = pizzas
            };

            pizzeria.Bills.Add(bill);

            return bill;
        }

        public List<Bill> GetBillsInDateRange(DateOnly from, DateOnly to)
        {
            if (to < from)
            {
                throw new ArgumentException($"`from` must be after `to`");
            }

            return pizzeria.Bills.Where(b =>
            {
                var date = DateOnly.FromDateTime(b.Time);
                return date >= from && date <= to;
            }).ToList();
        }
    }
}
