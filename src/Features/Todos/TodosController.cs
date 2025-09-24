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
}