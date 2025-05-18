using Beyond.Todo.Application;
using Beyond.Todo.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

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
            AnsiConsole.Write(new Rule("[blue]Todo List Application[/]").RuleStyle("grey"));
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an option:")
                    .AddChoices(new[] {
                        "1. Add Item",
                        "2. Update Item",
                        "3. Remove Item",
                        "4. Register Progression",
                        "5. Print Items",
                        "6. Exit"
                    }));

            try
            {
                switch (choice[0].ToString())
                {
                    case "1":
                        var title = AnsiConsole.Ask<string>("Enter [green]title[/]:");
                        var description = AnsiConsole.Ask<string>("Enter [green]description[/]:");
                        var categories = await todoList.GetCategories();
                        var category = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Select [green]category[/]:")
                                .AddChoices(categories));
                        todoList.AddItem(title, description, category);
                        todoList.PrintItems();
                        break;

                    case "2":
                        todoList.PrintItems(); // Print current items
                        var updateId = AnsiConsole.Ask<int>("Enter [green]item ID[/] to update:");
                        var newDescription = AnsiConsole.Ask<string>("Enter [green]new description[/]:");
                        todoList.UpdateItem(updateId, newDescription);
                        break;

                    case "3":
                        todoList.PrintItems(); // Print current items
                        var removeId = AnsiConsole.Ask<int>("Enter [green]item ID[/] to remove:");
                        todoList.RemoveItem(removeId);
                        break;

                    case "4":
                        todoList.PrintItems(); // Print current items
                        var progressId = AnsiConsole.Ask<int>("Enter [green]item ID[/]:");
                        var percent = AnsiConsole.Ask<decimal>("Enter [green]progress percentage[/] (0-100):");
                        todoList.RegisterProgression(progressId, DateTime.Now, percent);
                        break;

                    case "5":
                        todoList.PrintItems();
                        break;

                    case "6":
                        return;

                    default:
                        AnsiConsole.MarkupLine("[red]Invalid option. Please try again.[/]");
                        break;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            }
        }
    }
}
