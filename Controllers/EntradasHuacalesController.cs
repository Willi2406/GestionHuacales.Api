using GestionarHuacales.Api.Models;
using GestionarHuacales.Api.Services;
using GestionHuacales.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GestionHuacales.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntradasHuacalesController : ControllerBase
{
    private readonly HuacalesServices _huacalesServices;

    public EntradasHuacalesController(HuacalesServices huacalesServices)
    {
        _huacalesServices = huacalesServices;
    }

    // GET: api/EntradasHuacales (Listar)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntradasHuacales>>> GetEntradas()
    {
        var entradas = await _huacalesServices.Listar(e => true);
        return Ok(entradas);
    }

    // GET: api/EntradasHuacales/5 (Buscar por ID)
    [HttpGet("{id}")]
    public async Task<ActionResult<EntradasHuacales>> GetEntrada(int id)
    {
        var entrada = await _huacalesServices.Buscar(id);
        if (entrada == null)
        {
            return NotFound("No se encontro la entrada.");
        }
        return Ok(entrada);
    }

    // POST: api/EntradasHuacales (Crear usando DTO)
    [HttpPost]
    public async Task<ActionResult<EntradasHuacales>> PostEntrada([FromBody] EntradaHuacalesDto entradaDto)
    {
        var entrada = new EntradasHuacales
        {
            EntradaId = 0, 
            Fecha = DateTime.UtcNow,
            NombreCliente = entradaDto.NombreCliente,
            DetalleHuacales = entradaDto.Detalle.Select(h => new DetalleHuacales
            {
                TipoId = h.TipoId,
                Cantidad = h.Cantidad,
                Precio = h.Precio,
            }).ToList()
        };

        entrada.Cantidad = entrada.DetalleHuacales.Sum(d => d.Cantidad);
        entrada.Precio = entrada.DetalleHuacales.Sum(d => d.Cantidad * d.Precio);

        var guardado = await _huacalesServices.Guardar(entrada);
        if (!guardado)
        {
            return BadRequest("No se pudo guardar la entrada.");
        }

        return CreatedAtAction(nameof(GetEntrada), new { id = entrada.EntradaId }, entrada);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEntrada(int id, [FromBody] EntradaHuacalesDto entradaDto)
    {
        var entrada = new EntradasHuacales
        {
            EntradaId = id, 
            Fecha = DateTime.UtcNow,
            NombreCliente = entradaDto.NombreCliente,
            DetalleHuacales = entradaDto.Detalle.Select(h => new DetalleHuacales
            {
                EntradaId = id, 
                TipoId = h.TipoId,
                Cantidad = h.Cantidad,
                Precio = h.Precio,
            }).ToList()
        };

        entrada.Cantidad = entrada.DetalleHuacales.Sum(d => d.Cantidad);
        entrada.Precio = entrada.DetalleHuacales.Sum(d => d.Cantidad * d.Precio);

        var modificado = await _huacalesServices.Guardar(entrada);
        if (!modificado)
        {
            return NotFound("No se pudo modificar la entrada, verifique que el ID existe.");
        }

        return NoContent(); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEntrada(int id)
    {
        var eliminado = await _huacalesServices.Eliminar(id);
        if (!eliminado)
        {
            return NotFound("No se encontro la entrada para eliminar.");
        }
        return NoContent(); 
    }
}
