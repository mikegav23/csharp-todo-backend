using Microsoft.AspNetCore.Mvc;

namespace TodosApp.Features.Todos.Controllers;

public record ReturnMessage (string message);

[ApiController]
[Route("api/todos")]
public class TodosController() : ControllerBase
{
    [HttpGet]
    public ReturnMessage GetAll([FromQuery] string message = "hello")
    {
        return new ReturnMessage(message);
    }
}