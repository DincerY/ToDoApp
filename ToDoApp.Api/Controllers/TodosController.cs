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
    [Route("getalltodo")]
    public async Task<IActionResult> GetAllTodos()
    {
        var todos = await _context.Todos.ToListAsync();
        var result =JsonConvert.SerializeObject(todos);
        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddTodo([FromBody]TodoVM todo)
    {
        Todo addedTodo = new Todo()
        {
            UserId = todo.UserId,
            Title = todo.Title,
            Completed = todo.Completed
        };

        if (await _context.Todos.AnyAsync(t => t.Title == todo.Title))
        {
            return BadRequest("Yapılacak iş zaten mevcut");
        }
        
        
        await _context.Todos.AddAsync(addedTodo);
        var result = await _context.SaveChangesAsync();
        if (result == 1)
        {
            return Ok($"{addedTodo.UserId} Kullanıcı Kimlikli Görev Eklendi");
        }
        else
        {
            return BadRequest($"{addedTodo.UserId} Kullanıcı Kimlikli Görev Eklenemedi");
        }
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            return BadRequest($"{id} bu kimliğe ait bir görev bulunamadı");
        }
        _context.Todos.Remove(todo);
        var result = await _context.SaveChangesAsync();
        if (result == 1)
        {
            return Ok($"{todo.Id} Kullanıcı Kimlikli Görev Silindi");
        }
        else
        {
            return BadRequest($"{todo.Id} Kullanıcı Kimlikli Görev Silinemedi");
        }
    }

    [HttpGet]
    [Route("update/{id}")]
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
            return Ok($"{todo.Id} Kullanıcı Kimlikli Görev Güncellendi");
        }
        else
        {
            return BadRequest($"{todo.Id} Kullanıcı Kimlikli Görev Güncellenemedi");
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