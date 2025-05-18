using Beyond.Todo.Application;
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

    public async void AddItem(string title, string description, string category)
    {
        await _mediator.Send(new AddTodoItemCommand(title, description, category));
    }

    public async void UpdateItem(int id, string description)
    {
        await _mediator.Send(new UpdateItemCommand(id, description));
    }

    public async void RemoveItem(int id)
    {
        await _mediator.Send(new RemoveItemCommand(id));
    }

    public async void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        await _mediator.Send(new RegisterProgressionCommand(id, dateTime, percent));
    }

    public async void PrintItems()
    {
        var items = await _mediator.Send(new PrintItemsQuery());
        var table = new Table();

        table.AddColumn("Id");
        table.AddColumn("Title");
        table.AddColumn("Description");
        table.AddColumn("Category");
        table.AddColumn("Progress");
        table.AddColumn("Last Update");

        foreach (var item in items)
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

    public async Task<List<string>> GetCategories()
    {
        return await _mediator.Send(new GetCategoriesQuery());
    }
}
