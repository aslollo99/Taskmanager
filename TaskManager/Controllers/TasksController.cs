using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private static readonly List<TaskItem> Tasks = new();
    private readonly AppDbContext _context;
    
    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        // Ritorno tutti i task
        var tasks = _context.Tasks.ToList();
        return Ok(tasks);
    }

    [HttpPost]
    public IActionResult Add([FromBody] TaskItem  task)
    {
        _context.Tasks.Add(task);
        _context.SaveChanges();
        return Ok(new { message = "Task aggiunto", value = task });
    }

}