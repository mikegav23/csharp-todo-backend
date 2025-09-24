namespace TodosApp.Features.Todos;

public static class TodoMapping
{
    public static TodoResponse ToDto(this Todo t) => new TodoResponse(t.Id, t.Title, t.Notes, t.CreatedAt, t.Status);
}