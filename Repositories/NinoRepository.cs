
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Data;
using ResidenciaApp.Models;

namespace ResidenciaApp.Repositories;

public class NinoRepository : INinoRepository
{
    private readonly ApplicationDbContext _context;

    public NinoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // LISTAR Y BUSCAR SOLO REGISTROS ACTIVOS
    public async Task<List<NinoAdolescente>> ObtenerTodosAsync(
        string? busqueda = null)
    {
        var consulta = _context.NinosAdolescentes
            .AsNoTracking()
            .Where(n => n.Activo);

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            busqueda = busqueda.Trim();

            consulta = consulta.Where(n =>
                n.Nombre.Contains(busqueda) ||
                n.Apellido.Contains(busqueda) ||
                n.Dni.Contains(busqueda));
        }

        return await consulta
            .OrderBy(n => n.Apellido)
            .ToListAsync();
    }

    // OBTENER REGISTRO ACTIVO POR ID
    public async Task<NinoAdolescente?> ObtenerPorIdAsync(int id)
    {
        return await _context.NinosAdolescentes
            .FirstOrDefaultAsync(n =>
                n.IdNinoAdolescente == id &&
                n.Activo);
    }

    // CONSULTAR REGISTRO CON SUS TURNOS
    // Incluye registros inactivos para conservar
    // la consulta del historial.
    public async Task<NinoAdolescente?> ObtenerConTurnosAsync(int id)
    {
        return await _context.NinosAdolescentes
            .AsNoTracking()
            .Include(n => n.Turnos)
                .ThenInclude(t => t.EstadoTurno)
            .FirstOrDefaultAsync(n =>
                n.IdNinoAdolescente == id);
    }

    public async Task<bool> ExistePorDniAsync(string dni, int? excluirId = null)
    {
        dni = dni.Trim();

        return await _context.NinosAdolescentes
            .AnyAsync(n =>
                n.Dni == dni &&
                (!excluirId.HasValue || n.IdNinoAdolescente != excluirId.Value));
    }

    // AGREGAR NUEVO REGISTRO
    public async Task AgregarAsync(NinoAdolescente nino)
    {
        nino.Activo = true;

        _context.NinosAdolescentes.Add(nino);
        await _context.SaveChangesAsync();
    }

    // ACTUALIZAR REGISTRO EXISTENTE
    public async Task ActualizarAsync(NinoAdolescente nino)
    {
        if (!nino.Activo)
        {
            throw new InvalidOperationException(
                "No se puede editar un registro inactivo.");
        }

        await _context.SaveChangesAsync();
    }

    // BAJA LÓGICA
    // No elimina físicamente el NNyA.
    public async Task EliminarAsync(int id)
    {
        var nino = await ObtenerPorIdAsync(id);

        if (nino == null)
            return;

        nino.Activo = false;

        await _context.SaveChangesAsync();
    }

    // VERIFICAR SI EXISTE UN REGISTRO ACTIVO
    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.NinosAdolescentes
            .AnyAsync(n =>
                n.IdNinoAdolescente == id &&
                n.Activo);
    }
}
