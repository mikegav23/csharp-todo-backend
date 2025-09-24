using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TodosApp.Infrastructure.Data;

namespace TodosApp.Features.Todos;

[ApiController]
[Route("api/todos")]
public class TodosController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TodoDto>> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        take = Math.Clamp(take, 1, 100);

        return await db.Set<Todo>()
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Skip(skip)
            .Take(take)
            .Select(x => new TodoDto(
                x.Id,
                x.Title,
                x.Notes,
                x.CreatedAt,
                x.Status
            ))
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> Create([FromBody] CreateTodoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest("Todo title is required");
        }

        var todo = new Todo { Title = dto.Title.Trim(), Notes = dto.Notes?.Trim() };
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