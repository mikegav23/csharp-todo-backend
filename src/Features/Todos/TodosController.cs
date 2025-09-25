using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TodosApp.Infrastructure.Data;

namespace TodosApp.Features.Todos;

[ApiController]
[Route("api/todos")]
public class TodosController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TodoResponse>> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        take = Math.Clamp(take, 1, 100);

        return await db.Set<Todo>()
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Take(take)
            .Select(x => new TodoResponse(
                x.Id,
                x.Title,
                x.Notes,
                x.CreatedAt,
                x.Status
            ))
            .ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TodoResponse>> GetOne(Guid id)
    {
        var todo = await db.Set<Todo>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return todo == null ? NotFound() : Ok(todo.ToDto());
    }


    [HttpPost]
    public async Task<ActionResult<TodoResponse>> Create([FromBody] CreateTodoRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Title))
        {
            return BadRequest("Todo title is required");
        }

        var todo = new Todo { Title = req.Title.Trim(), Notes = req.Notes?.Trim() };
        try
        {
            db.Add(todo);
            await db.SaveChangesAsync();
            Console.WriteLine($"Todo added to database: {todo.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }

        return Ok();
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<TodoResponse>> Update(Guid id, [FromBody] UpdateTodoRequest req)
    {
        var todo = await db.Set<Todo>().FirstOrDefaultAsync(x => x.Id == id);
        if (todo == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(req.Title))
        {
            todo.Title = req.Title.Trim();
        }

        if (!string.IsNullOrWhiteSpace(req.Notes))
        {
            todo.Notes = req.Notes.Trim();
        }

        if (req.Status != null)
        {
            todo.Status = req.Status.Value;
        }


        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine($"Todo updated: {todo.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }

        return Ok(todo.ToDto());
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var todo = new Todo { Id = id };

        db.Remove(todo);
        await db.SaveChangesAsync();

        return NoContent();
    }
}