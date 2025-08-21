using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.RequestModel;
using TaskManager.Service;

namespace TaskManager.Controllers;

[ApiController]
[Route("[controller]")]
public class StrutturaController : ControllerBase
{
    private readonly StrutturaService _strutturaService;

    public StrutturaController(StrutturaService strutturaService)
    {
        _strutturaService = strutturaService;
    }

    [HttpGet]
    public IActionResult GetListStrutture()
    {
        return Ok(_strutturaService.GetList());
    }

    [HttpGet("{id}")]
    public IActionResult GetStruttura(int id)
    {
        var struttura = _strutturaService.GetById(id);
        if (struttura == null)
        {
            return NotFound(new {message = "Struttura non trovata"});
        }
        return Ok(struttura);
    }

    [HttpPost("add")]
    public IActionResult CreateStruttura([FromBody] StrutturaModelRequest strutturaRequest)
    {
        var struttura = _strutturaService.Create(strutturaRequest);
        if (struttura == null)
        {
            return UnprocessableEntity(new {message = "Struttura non Creata"});
        }
        
        return Ok(struttura);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateStruttura([FromBody] StrutturaModelRequest strutturaRequest, int id)
    {
        _strutturaService.Update(strutturaRequest, id);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteStruttura(int id)
    {
        _strutturaService.Delete(id);
        return Ok();
    }
}