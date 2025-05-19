using System.Collections.Generic;

namespace Beyond.Todo.Application.Models;

public class PaginatedTodoItemsDto
{
    public IEnumerable<TodoItemDto> Items { get; set; }
    public int TotalCount { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
}
