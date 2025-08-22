using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.RequestModel;
using TaskManager.Service;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class DoorController : ControllerBase
{
    private readonly DoorService _doorService;

    public DoorController(DoorService doorService)
    {
        _doorService = doorService;
    }

    [HttpGet]
    public IActionResult GetDoors()
    {
        return Ok(_doorService.GetDoors());
    }

    [HttpGet("{id}")]
    public IActionResult GetDoor(int id)
    {
        return Ok(_doorService.GetDoor(id));
    }

    [HttpPost("add")]
    public IActionResult PostDoor([FromBody] DoorModelRequest door)
    {
        return Ok(_doorService.AddDoor(door));
    }

    [HttpPut("{id}")]
    public IActionResult PutDoor([FromBody] DoorModelRequest door, int id)
    {
        _doorService.UpdateDoor(door, id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDoor(int id)
    {
        _doorService.DeleteDoor(id);
        return Ok();
    }
}