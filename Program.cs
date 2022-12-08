using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace PubProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a list to store menu items
            List<MenuItem> menu = new List<MenuItem>();

            // Create a list to store customer orders
            List<Order> orders = new List<Order>();

            // Create a dictionary to store staff accounts
            Dictionary<string, string> staffAccounts = new Dictionary<string, string>();

            // Add some menu items to the menu
            menu.Add(new MenuItem("Pint of beer", 5.00, new List<string>{"wheat", "barley"}));
            menu.Add(new MenuItem("Glass of wine", 7.00, new List<string>{"sulfites"}));
            menu.Add(new MenuItem("Gin and tonic", 9.00, new List<string>{"juniper berries"}));
            menu.Add(new MenuItem("Vegi burger", 11.00, new List<string>{"soy", "gluten"}));

            // Add some staff accounts to the dictionary
            staffAccounts.Add("manager", "password");
            staffAccounts.Add("staff1", "password1");
            staffAccounts.Add("staff2", "password2");

            // Start a loop to allow the staff to enter customer orders
            bool done = false;
            while (!done)
            {
                // Print the menu
                Console.WriteLine("Menu:");
                foreach (MenuItem item in menu)
                {
                    Console.WriteLine($"{item.Name}: ${item.Price} ({string.Join(", ", item.Allergens)})");
                }

                // Ask the staff to login
                Console.Write("Enter username: ");
                string username = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                // Check if the login is valid
                if (staffAccounts.ContainsKey(username) && staffAccounts[username] == password)
                {
                    // Ask the staff to enter a customer order
                    Console.Write("Enter customer name: ");
                    string customerName = Console.ReadLine();
                    Console.Write("Enter menu item name: ");
                    string menuItemName = Console.ReadLine();

                    // Find the menu item with the specified name
                    MenuItem selectedItem = menu.Find(item => item.Name == menuItemName);

                    // Add the customer order to the list of orders
                    orders.Add(new Order(customerName, selectedItem));

                    // Ask the staff if they want to enter another order
                    Console.Write("Enter another order? (Y/N): ");
                    string anotherOrder = Console.ReadLine();
                    if (anotherOrder.ToUpper() == "N")
                    {
                        done = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid login. Please try again.");
                }
            }

            // Print the customer orders in a receipt style
            Console.WriteLine("Receipt:");
            foreach (Order order in orders)
            {
                Console.WriteLine($"{order.CustomerName}: {order.Item.Name} - ${order.Item.Price}");
            }

            Console.WriteLine("Thank you for using the pub program!");
        }
    }

    // MenuItem class to represent a menu item
    class MenuItem
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public List<string> Allergens { get; set; }

        public MenuItem(string name, double price, List<string> allergens)
        {
            Name = name;
            Price = price;
            Allergens = allergens;
        }
    }

    // Order class to represent a customer order
    class Order
    {
        public string CustomerName { get; set; }
        public MenuItem Item { get; set; }

        public Order(string customerName, MenuItem item)
        {
            CustomerName = customerName;
            Item = item;
        }
    }
}

