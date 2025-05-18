using Beyond.Todo.Application;
using MediatR;

namespace Beyond.Todo.Console;

public class TodoList : ITodoList
{
    private readonly IMediator _mediator;

    public TodoList(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async void AddItem(int id, string title, string description, string category)
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
        foreach (var item in items)
        {
            System.Console.WriteLine($"Id: {item.Id}");
            System.Console.WriteLine($"Title: {item.Title}");
            System.Console.WriteLine($"Description: {item.Description}");
            System.Console.WriteLine($"Category: {item.Category}");
            System.Console.WriteLine("Progressions:");
            foreach (var progression in item.Progressions)
            {
                System.Console.WriteLine($"  Date: {progression.Date}, Progress: {progression.CumulativePercent}%");
            }
            System.Console.WriteLine("-------------------");
        }
    }
}
