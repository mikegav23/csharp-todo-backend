namespace TodosApp.Features.Todos;

public static class TodoMapping
{
    public static TodoDto ToDto(this Todo t) => new (t.Id, t.Title, t.Notes, t.CreatedAt, t.Status);
}