using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using GestionarHuacales.Api.DAL;
using GestionarHuacales.Api.Models;

namespace GestionarHuacales.Api.Services;

public class HuacalesServices (IDbContextFactory<Contexto> DbFactory)
{
    private async Task AfectarExistencia(ICollection<DetalleHuacales> detalle, TipoOperacion tipoOperacion)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        foreach (var item in detalle)
        {
            var tipoHuacal = await contexto.TipoHuacales
                .SingleAsync(t => t.TipoId == item.TipoId);

            var cantidadEntrada = item.Cantidad;

            if (tipoOperacion == TipoOperacion.Suma)
                tipoHuacal.Existencia += cantidadEntrada; 
            else
                tipoHuacal.Existencia -= cantidadEntrada;

            await contexto.SaveChangesAsync();
        }
    }
    public async Task<bool> Existe (int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Huacales.AnyAsync(a => a.EntradaId == id);
    }

    public async Task<bool> Insertar(EntradasHuacales huacales)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Add(huacales);
        await AfectarExistencia(huacales.DetalleHuacales, TipoOperacion.Suma);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(EntradasHuacales huacales)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var original = await contexto.Huacales
            .Include(e => e.DetalleHuacales)
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.EntradaId == huacales.EntradaId);

        if (original == null) return false;

        await AfectarExistencia(original.DetalleHuacales, TipoOperacion.Resta);

        contexto.DetalleHuacales.RemoveRange(original.DetalleHuacales);

        contexto.Update(huacales);

        await AfectarExistencia(huacales.DetalleHuacales, TipoOperacion.Suma);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int EntradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entidad = await contexto.Huacales
            .Include(e => e.DetalleHuacales)
            .FirstOrDefaultAsync(e => e.EntradaId == EntradaId);

        if (entidad is null) return false;

        await AfectarExistencia(entidad.DetalleHuacales, TipoOperacion.Resta);

        contexto.DetalleHuacales.RemoveRange(entidad.DetalleHuacales);
        contexto.Huacales.Remove(entidad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<EntradasHuacales> Buscar(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
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
        using var ctx = await DbFactory.CreateDbContextAsync();
        return await ctx.Huacales
                        .Include(e => e.DetalleHuacales)
                        .Where(criterio)
                        .AsNoTracking()
                        .ToListAsync();
    }

    public async Task<List<TipoHuacales>> GetTipoHuacales()
    {
        using var ctx = await DbFactory.CreateDbContextAsync();
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