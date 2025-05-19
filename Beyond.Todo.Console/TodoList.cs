using Beyond.Todo.Application.Commands;
using Beyond.Todo.Application.Queries;
using MediatR;
using Spectre.Console;

namespace Beyond.Todo.Console;

public class TodoList : ITodoList
{
    private readonly IMediator _mediator;

    public TodoList(IMediator mediator)
    {
        _mediator = mediator;
    }

    private async Task<T> ExecuteWithErrorHandling<T>(Func<Task<T>> operation, string operationName)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error {operationName}: {ex.Message}[/]");
            return default;
        }
    }

    public async void AddItem(string title, string description, string categoryName)
    {
        var categories = await GetCategories();
        var category = categories.FirstOrDefault(c => c.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

        if (category == null)
        {
            AnsiConsole.MarkupLine($"[red]Error: Category '{categoryName}' not found[/]");
            return;
        }

        await ExecuteWithErrorHandling(
            () => _mediator.Send(new AddTodoItemCommand(title, description, category)),
            "adding item");
    }

    public async void UpdateItem(int id, string description)
    {
        await ExecuteWithErrorHandling(
            () => _mediator.Send(new UpdateItemCommand(id, description)),
            "updating item");
    }

    public async void RemoveItem(int id)
    {
        await ExecuteWithErrorHandling(
            () => _mediator.Send(new RemoveItemCommand(id)),
            "removing item");
    }

    public async void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        await ExecuteWithErrorHandling(
            () => _mediator.Send(new RegisterProgressionCommand(id, dateTime, percent)),
            "registering progression");
    }

    public async void PrintItems()
    {
        System.Console.Clear();
        var result = await ExecuteWithErrorHandling(
            () => _mediator.Send(new GetTodosQuery(0, 1000)),
            "getting items");

        if (result != null)
        {
            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Title");
            table.AddColumn("Description");
            table.AddColumn("Category");
            table.AddColumn("Progress");
            table.AddColumn("Last Update");

            foreach (var item in result.Items)
            {
                var lastProgression = item.Progressions.MaxBy(p => p.Date);
                var progress = lastProgression?.CumulativePercent ?? 0;
                var progressColor = progress >= 100 ? "green" : "yellow";

                var progressBar = $"{new string('#', (int)(progress / 10))}{new string('-', 10 - (int)(progress / 10))}";
                var progressText = progress >= 100
                    ? "[green]Complete[/]"
                    : $"[{progressColor}]{progressBar} {progress}%[/]";

                var row = new string[]
                {
                item.Id.ToString(),
                item.Title,
                item.Description,
                $"[blue]{item.Category}[/]",
                progressText,
                lastProgression?.Date.ToString("yyyy-MM-dd HH:mm") ?? "No progress"
                };

                table.AddRow(row);
            }

            AnsiConsole.Write(table);
        }
    }

    public async Task<List<string>> GetCategories()
    {
        var categories = await ExecuteWithErrorHandling(
            () => _mediator.Send(new GetCategoriesQuery()),
            "getting categories");

        if (categories != null)
        {
            return categories.Select(c => c.Name).ToList();
        }

        return new List<string>();
    }
}
