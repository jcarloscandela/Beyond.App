namespace Beyond.Todo.WebApp.Client.Models;

using System.ComponentModel.DataAnnotations;

public class CreateTodoDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;
}
