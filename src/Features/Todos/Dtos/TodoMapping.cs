using TodosApp.Features.Todos.Domain;

namespace TodosApp.Features.Todos.Dtos;

public static class TodoMapping
{
    public static TodoDto ToDto(this Todo t) => new (t.Id, t.Title, t.Notes, t.CreatedAt, t.Status);
}