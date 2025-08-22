
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.RequestModel;
using TaskManager.Service;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService userService;
    private readonly QrCodeService _qrCodeService;

    public UserController(UserService userService, QrCodeService qrCodeService)
    {
        this.userService = userService;
        _qrCodeService = qrCodeService;
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = userService.GetById(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(userService.GetAll());
    }

    [HttpPut("{id}")]
    public IActionResult PutUser(int id,[FromBody] UserModelRequest user)
    {
        userService.Update(id, user);
        return Ok();
    }

    [HttpPost("add")]
    public IActionResult AddUser([FromBody] UserModelRequest user)
    {
        return Ok(userService.Add(user));
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        userService.Delete(id);
        return Ok();
    }

    [HttpGet("{id}/qrcodes")]
    public IActionResult GetQrCodes(int id)
    {
        return Ok(_qrCodeService.GetQrCodesByUserId(id));
    }

    [HttpPost("{id}/qrcodes")]
    public IActionResult AddQrCodesUser(int id, [FromBody] QrCodeModelRequest qrCodeModel)
    {
        return Ok(_qrCodeService.AddQrCodeUser(id, qrCodeModel));
    }
}