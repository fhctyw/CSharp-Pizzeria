﻿using System.Reflection;
using System.Text.RegularExpressions;

namespace Pizza
{
    public class UserInterface : IDisposable
    {
        private ICustomerService customerService;
        private IPizzeriaService pizzeriaService;

        public UserInterface(
            IServiceFactory serviceFactory)
        {
            customerService = serviceFactory.CreateCustomerService();
            pizzeriaService = serviceFactory.CreatePizzeriaService();

            Load();
        }
        private void Load()
        {
            try
            {
                customerService.LoadCustomers(UserInterfaceHelpers.DefaultCustomerFileName);
                pizzeriaService.LoadPizzeria(UserInterfaceHelpers.DefaultPizzeriaFileName);

                PrintColorText("DB loaded successfully", ConsoleColor.Green);
            }
            catch (DirectoryNotFoundException)
            {
                PrintColorText("DB hasn't created yet", ConsoleColor.Cyan);
            }
            catch (Exception ex)
            {
                PrintColorText($"DB cannot be loaded {ex.Message}", ConsoleColor.Red);
            }
        }
        private void Save()
        {
            try
            {
                customerService.SaveCustomers(UserInterfaceHelpers.DefaultCustomerFileName);
                pizzeriaService.SavePizzeria(UserInterfaceHelpers.DefaultPizzeriaFileName);

                PrintColorText("DB saved successfully", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                PrintColorText($"DB cannot be saved {ex.Message}", ConsoleColor.Red);
            }
        }
        public void Dispose()
        {
            Save();
        }
        public void Run()
        {
            NavigateMenu(true,
                GetBillsInDateRange,
                SearchByIngredients,
                BuyPizza,
                UseCustomerService,
                UsePizzaService);
        }
        private void UseCustomerService()
        {
            NavigateMenu(false,
                SortByName,
                SortByMoney,
                LoadCustomers,
                SaveCustomers,
                GetCustomers,
                AddCustomer,
                GetCustomer,
                ChangeCustomer,
                RemoveCustomer);
        }
        #region Customer Methods
        private void LoadCustomers()
        {
            Console.Write($"Enter file name(default, {UserInterfaceHelpers.DefaultCustomerFileName}): ");
            string? fileName = Console.ReadLine();

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = UserInterfaceHelpers.DefaultCustomerFileName;
            }

            customerService.LoadCustomers(fileName);

        }
        private void SaveCustomers()
        {
            Console.Write($"Enter file name(default, {UserInterfaceHelpers.DefaultCustomerFileName}): ");
            string? fileName = Console.ReadLine();

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = UserInterfaceHelpers.DefaultCustomerFileName;
            }

            customerService.SaveCustomers(fileName);
        }
        private void GetCustomers()
        {
            DisplayTableProperties(customerService.GetCustomers());
        }
        private void AddCustomer()
        {
            Console.Write("Enter customer name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter customer address: ");
            string address = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter customer money: ");
            decimal money = decimal.Parse(Console.ReadLine() ?? "0");
            Customer customer = new()
            {
                Name = name,
                Address = address,
                Money = money
            };
            customerService.AddCustomer(customer);
        }
        private void GetCustomer()
        {
            Console.Write("Enter customer name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Customer customer = customerService.GetCustomer(name);

            Console.WriteLine();
            Console.WriteLine($"Name: {customer.Name}");
            Console.WriteLine($"Address: {customer.Address}");
            Console.WriteLine($"Money: {customer.Money:0.00}$");
            Console.WriteLine();
        }
        private void RemoveCustomer()
        {
            Console.Write("Enter customer name: ");
            string name = Console.ReadLine() ?? string.Empty;
            customerService.RemoveCustomer(name);
        }
        private void ChangeCustomer()
        {
            Console.Write("Enter customer name to be changed: ");
            string name = Console.ReadLine() ?? string.Empty;

            Customer foundCustomer = customerService.GetCustomer(name);

            Console.Write($"Enter new customer address(defualt, {foundCustomer.Address}): ");
            string address = Console.ReadLine() ?? string.Empty;

            Console.Write($"Enter new customer money(default, {foundCustomer.Money}): ");
            decimal money = decimal.Parse(Console.ReadLine() ?? "0");

            Customer customer = new()
            {
                Name = name,
                Address = address,
                Money = money
            };
            customerService.ChangeCustomer(name, customer);
        }
        private void SortByName()
        {
            Console.Write($"Enter is descending: ");
            bool isDescending = bool.Parse(Console.ReadLine() ?? "false");
            customerService.SortCustomersByName(isDescending);
            GetCustomers();
        }
        private void SortByMoney()
        {
            Console.Write($"Enter is descending: ");
            bool isDescending = bool.Parse(Console.ReadLine() ?? "false");
            customerService.SortCustomersByMoney(isDescending);
            GetCustomers();
        }

        #endregion  
        private void UsePizzaService()
        {
            NavigateMenu(false,
                LoadPizzeria,
                SavePizzeria,
                GetPizzeria,
                GetIngredient,
                CreateIngredient,
                AddIngredients,
                AddPizzaToMenu);
        }

        #region Pizzeria Methods
        private void LoadPizzeria()
        {
            Console.Write($"Enter file name(default, {UserInterfaceHelpers.DefaultPizzeriaFileName}): ");
            string? fileName = Console.ReadLine();

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = UserInterfaceHelpers.DefaultPizzeriaFileName;
            }

            pizzeriaService.LoadPizzeria(fileName);
        }
        private void SavePizzeria()
        {
            Console.Write($"Enter file name(default, {UserInterfaceHelpers.DefaultPizzeriaFileName}): ");
            string? fileName = Console.ReadLine();

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = UserInterfaceHelpers.DefaultPizzeriaFileName;
            }

            pizzeriaService.SavePizzeria(fileName);
        }
        private void GetPizzeria()
        {
            Pizzeria pizzeria = pizzeriaService.GetPizzeria();
            Console.WriteLine();
            Console.WriteLine("Menu:");
            DisplayTableProperties(pizzeria.Menu, n => (string)n, p =>
                new string(string.Join(",", ((Dictionary<string, int>)p).Keys).Take(30).ToArray()),
                p => ((decimal)p).ToString() + "$"
            );
            Console.WriteLine();
            Console.WriteLine("Bills:");
            DisplayTableProperties(pizzeria.Bills, c => ((Customer)c).Name, d => ((DateTime)d).ToString(), p => p.ToString() + "$",
                p => string.Join(",", ((List<StandardPizza>)p).Select(pizza => pizza.Name)));
            Console.WriteLine();
            Console.WriteLine("Available Ingredients:");
            DisplayTableDictionary(pizzeria.AvailableIngredients, "Ingredient", "Count", i => i.Name, c => c.ToString());
            Console.WriteLine();
        }
        private Dictionary<string, int> AskIngredients()
        {
            Dictionary<string, int> ingredients = new();

            for (; ; )
            {
                Console.Write("Enter ingredient name: ");
                string name = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter ingredient count: ");

                ingredients[name] = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Enter if continue(y/n): ");
                if (Console.ReadLine() == "n")
                {
                    break;
                }
            }
            return ingredients;
        }
        private void AddPizzaToMenu()
        {
            Console.Write("Enter pizza name: ");
            string name = Console.ReadLine() ?? string.Empty;
            var neededIngredients = AskIngredients();
            Console.Write("Enter pizze price: ");
            decimal price = decimal.Parse(Console.ReadLine() ?? "0");

            StandardPizza orderedPizza = new()
            {
                Name = name,
                NeededIngredients = neededIngredients,
                Price = price,
            };
            pizzeriaService.AddPizza(orderedPizza);
        }
        private void CreateIngredient()
        {
            Console.Write("Enter ingredient name: ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter ingredient price: ");
            decimal price = decimal.Parse(Console.ReadLine() ?? "0");

            Ingredient ingredient = new()
            {
                Name = name,
                Price = price,
            };
            pizzeriaService.AddAvailableIngredient(ingredient);

            PrintColorText("Ingredient created successfully", ConsoleColor.Green);
        }
        private void GetIngredient()
        {
            Console.Write("Enter ingredient name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Ingredient ingredient = pizzeriaService.GetAvailableIngredient(name);

            Console.WriteLine();
            Console.WriteLine($"Name: {ingredient.Name}");
            Console.WriteLine($"Price: {ingredient.Price:0.00}$");
            Console.WriteLine();
        }
        private void AddIngredients()
        {
            Console.Write("Enter ingredient name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter ingredient count: ");
            int count = int.Parse(Console.ReadLine() ?? "0");

            pizzeriaService.AddIngredients(name, count);
            PrintColorText($"Ingredient added {count} count successfully", ConsoleColor.Green);
        }
        private Dictionary<string, int> AskOrderedPizzas()
        {
            var orderedPizzas = new Dictionary<string, int>();

            for (; ; )
            {
                Console.Write("Enter pizza name: ");
                string name = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter pizza count: ");

                orderedPizzas[name] = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Enter if continue(y/n): ");
                if (Console.ReadLine() == "n")
                {
                    break;
                }
            }
            return orderedPizzas;
        }
        private Dictionary<CustomPizza, int> AskCustomPizzas()
        {
            var customPizzas = new Dictionary<CustomPizza, int>();

            for (; ; )
            {
                Console.Write("Enter custom pizza name: ");
                string name = Console.ReadLine() ?? string.Empty;

                StandardPizza orderedPizza = pizzeriaService.GetPizza(name);

                Console.WriteLine("Enter additional ingredients: ");
                var additionalIngredients = AskIngredients();

                Console.Write("Enter pizza count: ");

                customPizzas[new()
                {
                    Name = name,
                    NeededIngredients = orderedPizza.NeededIngredients,
                    Price = orderedPizza.Price,
                    AdditionalIngredients = additionalIngredients
                }] = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Enter if continue(y/n): ");
                if (Console.ReadLine() == "n")
                {
                    break;
                }
            }

            return customPizzas;
        }
        private void SearchByIngredients()
        {
            Console.WriteLine("Enter ingredients: ");
            var ingredients = new List<string>();

            for (; ; )
            {
                Console.Write("Enter ingredient name: ");
                string name = Console.ReadLine() ?? string.Empty;

                ingredients.Add(name);

                Console.Write("Enter if continue(y/n): ");
                if (Console.ReadLine() == "n")
                {
                    break;
                }
            }

            DisplayTableProperties(pizzeriaService.GetPizzasByIngredients(ingredients), n => (string)n, p =>
                new string(string.Join(",", ((Dictionary<string, int>)p).Keys).Take(30).ToArray()),
                p => ((decimal)p).ToString() + "$"
            );

        }
        private void BuyPizza()
        {
            Console.Write("Enter customer name: ");
            string customerName = Console.ReadLine() ?? string.Empty;

            var orderedPizzas = AskOrderedPizzas();
            Console.Write("Enter if you want to add cutome pizza(y/n): ");
            var customPizzas = new Dictionary<CustomPizza, int>();
            if (Console.ReadLine() == "y")
            {
                customPizzas = AskCustomPizzas();
            }

            Bill bill = pizzeriaService.SellPizza(customerName, new() { OrderedPizzas = orderedPizzas, CustomPizzas = customPizzas });

            Console.WriteLine();
            PrintColorText($"Pizzas sold successfully", ConsoleColor.Green);
            Console.WriteLine();
            Console.WriteLine("Bill:");
            Console.WriteLine($"Customer: {bill.Customer}");
            Console.WriteLine($"Pizzas: {string.Join(",", bill.Pizzas.Select(p => p.Name))}");
            Console.WriteLine($"Time: {bill.Time}");
            Console.WriteLine($"Total price: {bill.TotalPrice}");
            Console.WriteLine();
        }

        private void GetBillsInDateRange()
        {
            Console.WriteLine("format: yyyy-MM-dd");
            Console.Write("Write `from` that date to take: ");
            var from = DateOnly.Parse(Console.ReadLine() ?? string.Empty);
            Console.Write("Write `to` that date to take: ");
            var to = DateOnly.Parse(Console.ReadLine() ?? string.Empty);

            DisplayTableProperties(pizzeriaService.GetBillsInDateRange(from, to));
        }

        #endregion

        #region Help Methods
        private string FormatMethodName(Action action) => Regex.Replace(action.Method.Name, "(?<!^)([A-Z])", " $1");
        private void DisplayTableProperties<T>(IEnumerable<T> enumerable, params Func<object, string>[] selectors)
        {
            Type type = enumerable.GetType().GenericTypeArguments[0];
            PropertyInfo[] properties = type.GetProperties();
            int[] offsets = properties.Select(p => p.Name.Length).ToArray();
            if (enumerable.Any())
            {
                foreach (var element in enumerable)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var obj = properties[i].GetValue(element);
                        var stringElement = i < selectors.Length && obj != null
                            ? selectors[i](obj)
                            : obj?.ToString() ?? string.Empty;
                        offsets[i] = Math.Max(offsets[i], stringElement == null ? 0 : stringElement.Length);
                    }
                }
            }
            int widthBorder = offsets.Sum() + offsets.Length * 2 + 1;
            Console.WriteLine(new string('-', widthBorder));
            Console.Write('|');
            for (int i = 0; i < properties.Length; i++)
            {
                Console.Write(properties[i].Name.PadRight(offsets[i] + 1));
                if (i != properties.Length - 1)
                {
                    Console.Write('|');
                }
            }
            Console.WriteLine('|');
            Console.WriteLine(new string('-', widthBorder));
            foreach (var element in enumerable)
            {
                Console.Write('|');
                for (int i = 0; i < properties.Length; i++)
                {
                    var obj = properties[i].GetValue(element);
                    var stringElement = i < selectors.Length && obj != null
                        ? selectors[i](obj)
                        : obj?.ToString() ?? string.Empty;
                    Console.Write(stringElement.PadRight(offsets[i] + 1));
                    if (i != properties.Length - 1)
                    {
                        Console.Write('|');
                    }
                }
                Console.WriteLine('|');
            }
            Console.WriteLine(new string('-', widthBorder));
        }
        private void DisplayTableDictionary<K, V>(IDictionary<K, V> dictionary, string key, string value, Func<K, string> keySelector, Func<V, string> valueSelector)
        {
            int[] offsets = { key.Length, value.Length };
            if (dictionary.Any())
            {
                foreach (var pair in dictionary)
                {
                    offsets[0] = Math.Max(offsets[0], keySelector(pair.Key).Length);
                    offsets[1] = Math.Max(offsets[1], valueSelector(pair.Value).Length);
                }
            }
            int widthBorder = offsets.Sum() + offsets.Length * 2 + 1;
            Console.WriteLine(new string('-', widthBorder));
            Console.WriteLine($"|{key.PadRight(offsets[0] + 1)}|{value.PadRight(offsets[1] + 1)}|");
            Console.WriteLine(new string('-', widthBorder));
            foreach (var pair in dictionary)
            {
                Console.WriteLine($"|{keySelector(pair.Key).PadRight(offsets[0] + 1)}|{valueSelector(pair.Value).PadRight(offsets[1] + 1)}|");
            }
            Console.WriteLine(new string('-', widthBorder));
        }
        private void DisplayChoices(bool isExit, params Action[] actions)
        {
            Console.WriteLine($"[0] {(isExit ? "Exit" : "Previous")}");
            for (int i = 0; i < actions.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {FormatMethodName(actions[i])}");
            }
        }
        private void NavigateMenu(bool isExit, params Action[] actions)
        {
            int choice;
            do
            {
                DisplayChoices(isExit, actions);
                Console.Write("Enter choice: ");
                try
                {
                    choice = int.Parse(Console.ReadLine() ?? "0");
                }
                catch (Exception)
                {
                    choice = -1;

                    Console.WriteLine();

                    PrintColorText("Invalid input", ConsoleColor.Red);
                    continue;
                }
                Console.WriteLine();
                int correctChoice = choice - 1;
                if (correctChoice >= 0 && correctChoice < actions.Length)
                {
                    try
                    {
                        actions[correctChoice].Invoke();
                    }
                    catch (Exception ex)
                    {
                        PrintColorText(ex.Message, ConsoleColor.Red);
                    }
                }
            } while (choice != 0);
        }
        private void PrintColorText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        #endregion
    }
}
