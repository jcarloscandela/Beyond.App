using Beyond.Todo.Application;
using Beyond.Todo.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Beyond.Todo.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        // Add application services
        services.AddApplication();
        services.AddInfrastructure();
        services.AddTransient<ITodoList, TodoList>();

        var serviceProvider = services.BuildServiceProvider();
        var todoList = serviceProvider.GetRequiredService<ITodoList>();

        while (true)
        {
            System.Console.WriteLine("\nTodo List Application");
            System.Console.WriteLine("1. Add Item");
            System.Console.WriteLine("2. Update Item");
            System.Console.WriteLine("3. Remove Item");
            System.Console.WriteLine("4. Register Progression");
            System.Console.WriteLine("5. Print Items");
            System.Console.WriteLine("6. Exit");
            System.Console.Write("Choose an option: ");

            var choice = System.Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        System.Console.Write("Enter title: ");
                        var title = System.Console.ReadLine() ?? "";
                        System.Console.Write("Enter description: ");
                        var description = System.Console.ReadLine() ?? "";
                        System.Console.Write("Enter category: ");
                        var category = System.Console.ReadLine() ?? "";
                        todoList.PrintItems(); // Print before action
                        todoList.AddItem(title, description, category);
                        break;

                    case "2":
                        todoList.PrintItems(); // Print current items
                        System.Console.Write("Enter item ID to update: ");
                        if (int.TryParse(System.Console.ReadLine(), out int updateId))
                        {
                            System.Console.Write("Enter new description: ");
                            var newDescription = System.Console.ReadLine() ?? "";
                            todoList.UpdateItem(updateId, newDescription);
                        }
                        break;

                    case "3":
                        todoList.PrintItems(); // Print current items
                        System.Console.Write("Enter item ID to remove: ");
                        if (int.TryParse(System.Console.ReadLine(), out int removeId))
                        {
                            todoList.RemoveItem(removeId);
                        }
                        break;

                    case "4":
                        todoList.PrintItems(); // Print current items
                        System.Console.Write("Enter item ID: ");
                        if (int.TryParse(System.Console.ReadLine(), out int progressId))
                        {
                            System.Console.Write("Enter progress percentage (0-100): ");
                            if (decimal.TryParse(System.Console.ReadLine(), out decimal percent))
                            {
                                todoList.RegisterProgression(progressId, DateTime.Now, percent);
                            }
                        }
                        break;

                    case "5":
                        todoList.PrintItems();
                        break;

                    case "6":
                        return;

                    default:
                        System.Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
