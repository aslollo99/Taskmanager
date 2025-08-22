using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;
using TaskManager.RequestModel;
using TaskManager.Service;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class QrCodeController : ControllerBase
{
    private readonly QrCodeService _qrCodeService;

    public QrCodeController(QrCodeService qrCodeService)
    {
        _qrCodeService = qrCodeService;
    }

    [HttpPut("{id}")]
    public IActionResult UpdateStatoQrCode(int id, [FromBody] QrCodeModelRequest qrCodeModel)
    {
        _qrCodeService.UpdateStatoQrCode(id, qrCodeModel);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteStatoQrCode(int id)
    {
        _qrCodeService.Delete(id);
        return Ok();
    }
    
}