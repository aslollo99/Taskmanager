using Microsoft.AspNetCore.Mvc;
using TaskManager.RequestModel;
using TaskManager.Service;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class AccessRightController : ControllerBase
{
    private readonly AccessRightService  _accessRightService;

    public AccessRightController(AccessRightService accessRightService)
    {
        _accessRightService = accessRightService;
    }

    [HttpGet("byqrcode/{id}")]
    public IActionResult GetByQrcodeId(int id)
    {
        return Ok(_accessRightService.GetAccessRightsByQrCodeId(id));
    }
    
    [HttpPut("byqrcode/{id}")]
    public IActionResult GetByQrcodeId(int id, [FromBody] List<AccessRightModelRequest> accessRightModel)
    {
        _accessRightService.UpdateDoorByQrCodeId(id, accessRightModel);
        return Ok();
    }
}