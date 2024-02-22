using System.ComponentModel.DataAnnotations;
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
    [Route("/getalltodo")]
    public async Task<IActionResult> GetAllTodos()
    {
        var todos = await _context.Todos.ToListAsync();
        var result =JsonConvert.SerializeObject(todos);
        return Ok(result);
    }

    [HttpPost]
    [Route("/add")]
    public async Task<IActionResult> AddTodo([FromBody]TodoVM todo)
    {
        Todo addedTodo = new Todo()
        {
            UserId = todo.UserId,
            Title = todo.Title,
            Completed = todo.Completed
        };
        await _context.Todos.AddAsync(addedTodo);
        var result = await _context.SaveChangesAsync();
        if (result == 1)
        {
            return Ok($"{addedTodo.UserId} Kullanıcı Kimlikli Ürün Eklendi");
        }
        else
        {
            return BadRequest($"{addedTodo.UserId} Kullanıcı Kimlikli Ürün Eklenemedi");
        }
    }

    [HttpDelete]
    [Route("/delete/{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        _context.Todos.Remove(todo);
        var result = await _context.SaveChangesAsync();
        if (result == 1)
        {
            return Ok("Silindi");
        }
        else
        {
            return BadRequest("Silinemedi");
        }
    }

    [HttpGet]
    [Route("/update/{id}")]
    public async Task<IActionResult> UpdateTodosComplete(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (!todo.Completed)
        {
            todo.Completed = true;
        }
        var result = await _context.SaveChangesAsync();
        if (result == 1)
        {
            return Ok("Güncellendi");
        }
        else
        {
            return BadRequest("Güncellenemedi");
        }
    }

}

public class TodoVM
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public bool Completed { get; set; }
}