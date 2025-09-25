namespace TodosApp.Features.Todos;

public record TodoResponse(Guid Id, string Title, string? Notes, DateTimeOffset CreatedAt, TodoStatus Status);

public class CreateTodoRequest
{
    public string Title { get; set; } = default!;
    public string? Notes { get; set; }
}

public class UpdateTodoRequest
{
    public string? Title { get; set; } = default!;
    public string? Notes { get; set; }
    public TodoStatus? Status { get; set; }
}