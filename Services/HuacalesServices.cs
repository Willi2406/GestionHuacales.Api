using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using GestionarHuacales.Api.DAL;
using GestionarHuacales.Api.Models;

namespace GestionarHuacales.Api.Services;

public class HuacalesServices(IDbContextFactory<Contexto> dbFactory)
{
    
    private async Task AfectarExistencia(Contexto contexto, ICollection<DetalleHuacales> detalle, TipoOperacion tipoOperacion)
    {
        foreach (var item in detalle)
        {
            var tipoHuacal = await contexto.TipoHuacales
                .SingleAsync(t => t.TipoId == item.TipoId);

            if (tipoOperacion == TipoOperacion.Suma)
                tipoHuacal.Existencia += item.Cantidad;
            else
                tipoHuacal.Existencia -= item.Cantidad;
        }
    }

    public async Task<bool> Existe(int id)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();
        return await contexto.Huacales.AnyAsync(a => a.EntradaId == id);
    }

    public async Task<bool> Insertar(EntradasHuacales huacales)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();

        await AfectarExistencia(contexto, huacales.DetalleHuacales, TipoOperacion.Suma);

        contexto.Add(huacales);

        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<bool> Modificar(EntradasHuacales huacales)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();

        var entradaActual = await contexto.Huacales
            .Include(e => e.DetalleHuacales)
            .FirstOrDefaultAsync(e => e.EntradaId == huacales.EntradaId);

        if (entradaActual == null)
        {
            return false;
        }

        await AfectarExistencia(contexto, entradaActual.DetalleHuacales, TipoOperacion.Resta);

        contexto.DetalleHuacales.RemoveRange(entradaActual.DetalleHuacales);

        entradaActual.NombreCliente = huacales.NombreCliente;
        entradaActual.Fecha = huacales.Fecha;
        entradaActual.Cantidad = huacales.Cantidad;
        entradaActual.Precio = huacales.Precio;
        entradaActual.DetalleHuacales = huacales.DetalleHuacales;

        await AfectarExistencia(contexto, entradaActual.DetalleHuacales, TipoOperacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int entradaId)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();

        var entidad = await contexto.Huacales
            .Include(e => e.DetalleHuacales)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);

        if (entidad is null) return false;

        await AfectarExistencia(contexto, entidad.DetalleHuacales, TipoOperacion.Resta);

        contexto.Huacales.Remove(entidad);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<EntradasHuacales?> Buscar(int entradaId)
    {
        await using var contexto = await dbFactory.CreateDbContextAsync();
        return await contexto.Huacales
            .Include(e => e.DetalleHuacales)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }

    public async Task<bool> Guardar(EntradasHuacales huacales)
    {
        if (!await Existe(huacales.EntradaId))
        {
            return await Insertar(huacales);
        }
        else
        {
            return await Modificar(huacales);
        }
    }

    public async Task<List<EntradasHuacales>> Listar(Expression<Func<EntradasHuacales, bool>> criterio)
    {
        using var ctx = await dbFactory.CreateDbContextAsync();
        return await ctx.Huacales
                        .Include(e => e.DetalleHuacales)
                        .Where(criterio)
                        .AsNoTracking()
                        .ToListAsync();
    }

    public async Task<List<TipoHuacales>> GetTipoHuacales()
    {
        using var ctx = await dbFactory.CreateDbContextAsync();
        return await ctx.TipoHuacales
            .AsNoTracking()
            .ToListAsync();
    }

    public enum TipoOperacion
    {
        Suma = 1,
        Resta = 2
    }
}