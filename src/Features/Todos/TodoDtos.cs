namespace TodosApp.Features.Todos;

public record TodoDto(Guid Id, string Title, string? Notes, DateTimeOffset CreatedAt, TodoStatus Status);

public record CreateTodoDto(string Title, string? Notes);

public record UpdateTodoDto(string Title, string? Notes, TodoStatus Status);