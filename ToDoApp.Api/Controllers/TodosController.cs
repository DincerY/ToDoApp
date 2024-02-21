using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ToDoApp.Api.Repository;

namespace ToDoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
    private readonly TodoDbContext _context;

    public TodosController(TodoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTodos()
    {
        var todos = await _context.Todos.ToListAsync();
        var result =JsonConvert.SerializeObject(todos);
        return Ok(result);
    }

    [HttpPost]
    [Route("/add")]
    public async Task<IActionResult> AddTodo([FromBody]Todo todo)
    {
        await _context.Todos.AddAsync(todo);
        var result = await _context.SaveChangesAsync();
        return Ok(result == 1 ? "Eklendi" : "Eklenemedi");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTodo()
    {
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTodo()
    {
        return Ok();
    }

}