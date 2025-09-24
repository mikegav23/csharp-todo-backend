namespace TodosApp.Features.Todos.Domain;

public enum TodoStatus { Pending, Completed, Cancelled };

public class Todo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public TodoStatus Status { get; set; } = TodoStatus.Pending;
}