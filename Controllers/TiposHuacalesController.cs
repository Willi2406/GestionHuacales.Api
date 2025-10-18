using GestionarHuacales.Api.Models;
using GestionarHuacales.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionHuacales.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TiposHuacalesController : ControllerBase
{
    private readonly HuacalesServices _huacalesServices;

    public TiposHuacalesController(HuacalesServices huacalesServices)
    {
        _huacalesServices = huacalesServices;
    }

    // GET: api/TiposHuacales
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoHuacales>>> GetTiposHuacales()
    {
        try
        {
            var tipos = await _huacalesServices.GetTipoHuacales();
            return Ok(tipos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}