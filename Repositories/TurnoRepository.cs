
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Data;
using ResidenciaApp.Models;

namespace ResidenciaApp.Repositories;

public class TurnoRepository : ITurnoRepository
{
    private readonly ApplicationDbContext _context;

    public TurnoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Turno>> ObtenerTodosAsync(
        string? tab = null,
        string? tipo = null,
        string? search = null,
        int? estadoId = null,
        DateTime? fechaDesde = null,
        DateTime? fechaHasta = null)
    {
        var turnos = _context.Turnos
            .Include(t => t.NinoAdolescente)
            .Include(t => t.EstadoTurno)
            .AsQueryable();

        if (!string.IsNullOrEmpty(tab))
        {
            var hoy = DateTime.Today;

            switch (tab)
            {
                case "Próximos":
                    turnos = turnos.Where(t => t.Fecha >= hoy);
                    break;

                case "Pasados":
                    turnos = turnos.Where(t => t.Fecha < hoy);
                    break;

                case "Cancelados":
                    turnos = turnos.Where(t =>
                        t.EstadoTurno != null &&
                        t.EstadoTurno.Nombre == "Cancelado");
                    break;
            }
        }

        if (!string.IsNullOrEmpty(tipo))
            turnos = turnos.Where(t => t.TipoTurno == tipo);

        if (!string.IsNullOrEmpty(search))
        {
            turnos = turnos.Where(t =>
                (t.NinoAdolescente != null &&
                    (t.NinoAdolescente.Nombre.Contains(search) ||
                     t.NinoAdolescente.Apellido.Contains(search))) ||
                t.NombreProfesional.Contains(search) ||
                t.NombreInstitucion.Contains(search) ||
                t.TipoTurno.Contains(search));
        }

        if (estadoId.HasValue)
            turnos = turnos.Where(t =>
                t.IdEstadoTurno == estadoId.Value);

        if (fechaDesde.HasValue)
            turnos = turnos.Where(t =>
                t.Fecha >= fechaDesde.Value);

        if (fechaHasta.HasValue)
            turnos = turnos.Where(t =>
                t.Fecha <= fechaHasta.Value);

        return await turnos
            .OrderByDescending(t => t.Fecha)
            .ToListAsync();
    }

    public async Task<Turno?> ObtenerPorIdAsync(int id)
    {
        return await _context.Turnos
            .FirstOrDefaultAsync(t => t.IdTurno == id);
    }

    public async Task<Turno?> ObtenerConHistorialAsync(int id)
    {
        return await _context.Turnos
            .Include(t => t.NinoAdolescente)
            .Include(t => t.EstadoTurno)
            .Include(t => t.HistorialTurnos)
                .ThenInclude(h => h.EstadoTurno)
            .FirstOrDefaultAsync(t => t.IdTurno == id);
    }

    public async Task<List<NinoAdolescente>> ObtenerNinosAsync()
    {
        return await _context.NinosAdolescentes
            .OrderBy(n => n.Apellido)
            .ToListAsync();
    }

    public async Task<List<EstadoTurno>> ObtenerEstadosAsync()
    {
        return await _context.EstadosTurno
            .ToListAsync();
    }

    public async Task AgregarAsync(Turno turno)
    {
        _context.Turnos.Add(turno);

        _context.HistorialTurnos.Add(new HistorialTurno
        {
            Turno = turno,
            IdEstadoTurno = turno.IdEstadoTurno,
            FechaHora = DateTime.Now,
            Accion = "Creación",
            Observacion = "Turno creado"
        });

        await _context.SaveChangesAsync();
    }


    public async Task ActualizarAsync(Turno turno)
    {
        var entrada = _context.Entry(turno);

        if (entrada.State == EntityState.Detached)
        {
            throw new InvalidOperationException(
                "El turno debe recuperarse desde el repositorio antes de actualizarlo.");
        }

        // Obtener el estado original del turno
        var estadoAnteriorId = entrada
            .Property(t => t.IdEstadoTurno)
            .OriginalValue;

        var estadoNuevoId = turno.IdEstadoTurno;

        // Registrar historial solo si cambió el estado
        if (estadoAnteriorId != estadoNuevoId)
        {
            var estadoAnterior = await _context.EstadosTurno
                .FindAsync(estadoAnteriorId);

            var estadoNuevo = await _context.EstadosTurno
                .FindAsync(estadoNuevoId);

            _context.HistorialTurnos.Add(new HistorialTurno
            {
                IdTurno = turno.IdTurno,
                IdEstadoTurno = estadoNuevoId,
                FechaHora = DateTime.Now,
                Accion = "Cambio de estado",
                Observacion =
                    $"Estado cambiado de {estadoAnterior?.Nombre} a {estadoNuevo?.Nombre}"
            });
        }

        // Guardar los cambios del turno y su historial
        await _context.SaveChangesAsync();
    }


    public async Task EliminarAsync(int id)
    {
        var turno = await _context.Turnos.FindAsync(id);

        if (turno != null)
        {
            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Turnos
            .AnyAsync(t => t.IdTurno == id);
    }
}
