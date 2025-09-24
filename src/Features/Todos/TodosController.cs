using Microsoft.AspNetCore.Mvc;
using TodosApp.Infrastructure.Data;

namespace TodosApp.Features.Todos;

public record ReturnMessage (string message);

[ApiController]
[Route("api/todos")]
public class TodosController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public ReturnMessage GetAll([FromQuery] string message = "hello")
    {
        return new ReturnMessage(message);
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