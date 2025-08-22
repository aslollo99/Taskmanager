using Microsoft.AspNetCore.Mvc;
using TaskManager.RequestModel;
using TaskManager.Service;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] AuthModelRequest request)
    {
        var user = _userService.ValidateUser(request);

        if (user == null)
        {
            return Unauthorized(new {message = "Username or password invalidi"});
        }
        
        return Ok(new {message = "Login Effettuato", userId = user.Id, fullName = user.fullname, role = user.Role});
    }
}