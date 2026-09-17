using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Data;
using ResidenciaApp.Models;

namespace ResidenciaApp.Controllers;

[Authorize]
public class TurnosController : Controller
{
    private readonly ApplicationDbContext _context;

    public TurnosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? tab, string? tipo, string? search, int? estadoId, DateTime? fechaDesde, DateTime? fechaHasta)
    {
        ViewData["Tab"] = tab ?? "Todos";
        ViewData["TipoSeleccionado"] = tipo ?? "";
        ViewData["SearchString"] = search ?? "";
        ViewData["EstadoId"] = new SelectList(_context.EstadosTurno, "IdEstadoTurno", "Nombre", estadoId);

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
                    turnos = turnos.Where(t => t.EstadoTurno != null && t.EstadoTurno.Nombre == "Cancelado");
                    break;
            }
        }

        if (!string.IsNullOrEmpty(tipo))
            turnos = turnos.Where(t => t.TipoTurno == tipo);

        if (!string.IsNullOrEmpty(search))
            turnos = turnos.Where(t =>
                (t.NinoAdolescente != null && (t.NinoAdolescente.Nombre.Contains(search) || t.NinoAdolescente.Apellido.Contains(search))) ||
                t.NombreProfesional.Contains(search) ||
                t.NombreInstitucion.Contains(search) ||
                t.TipoTurno.Contains(search));

        if (estadoId.HasValue)
            turnos = turnos.Where(t => t.IdEstadoTurno == estadoId.Value);

        if (fechaDesde.HasValue)
            turnos = turnos.Where(t => t.Fecha >= fechaDesde.Value);

        if (fechaHasta.HasValue)
            turnos = turnos.Where(t => t.Fecha <= fechaHasta.Value);

        return View(await turnos.OrderByDescending(t => t.Fecha).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var turno = await _context.Turnos
            .Include(t => t.NinoAdolescente)
            .Include(t => t.EstadoTurno)
            .Include(t => t.HistorialTurnos).ThenInclude(h => h.EstadoTurno)
            .FirstOrDefaultAsync(t => t.IdTurno == id);

        if (turno == null) return NotFound();

        return View(turno);
    }

    public IActionResult Create(int? ninoId)
    {
        ViewData["IdNinoAdolescente"] = new SelectList(_context.NinosAdolescentes, "IdNinoAdolescente", "Apellido", ninoId);
        ViewData["IdEstadoTurno"] = new SelectList(_context.EstadosTurno, "IdEstadoTurno", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdNinoAdolescente,NombreProfesional,NombreInstitucion,IdEstadoTurno,TipoTurno,Fecha,Hora,Motivo,Observaciones")] Turno turno)
    {
        if (ModelState.IsValid)
        {
            _context.Add(turno);

            _context.HistorialTurnos.Add(new HistorialTurno
            {
                Turno = turno,
                IdEstadoTurno = turno.IdEstadoTurno,
                FechaHora = DateTime.Now,
                Accion = "Creación",
                Observacion = "Turno creado"
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["IdNinoAdolescente"] = new SelectList(_context.NinosAdolescentes, "IdNinoAdolescente", "Apellido", turno.IdNinoAdolescente);
        ViewData["IdEstadoTurno"] = new SelectList(_context.EstadosTurno, "IdEstadoTurno", "Nombre", turno.IdEstadoTurno);
        return View(turno);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var turno = await _context.Turnos.FindAsync(id);
        if (turno == null) return NotFound();

        ViewData["IdNinoAdolescente"] = new SelectList(_context.NinosAdolescentes, "IdNinoAdolescente", "Apellido", turno.IdNinoAdolescente);
        ViewData["IdEstadoTurno"] = new SelectList(_context.EstadosTurno, "IdEstadoTurno", "Nombre", turno.IdEstadoTurno);
        return View(turno);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdTurno,IdNinoAdolescente,NombreProfesional,NombreInstitucion,IdEstadoTurno,TipoTurno,Fecha,Hora,Motivo,Resultado,Observaciones")] Turno turno)
    {
        if (id != turno.IdTurno) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var turnoActual = await _context.Turnos.AsNoTracking().FirstOrDefaultAsync(t => t.IdTurno == id);
                if (turnoActual == null) return NotFound();

                var estadoAnteriorId = turnoActual.IdEstadoTurno;

                _context.Update(turno);

                if (estadoAnteriorId != turno.IdEstadoTurno)
                {
                    var estadoAnterior = await _context.EstadosTurno.FindAsync(estadoAnteriorId);
                    var estadoNuevo = await _context.EstadosTurno.FindAsync(turno.IdEstadoTurno);

                    _context.HistorialTurnos.Add(new HistorialTurno
                    {
                        IdTurno = turno.IdTurno,
                        IdEstadoTurno = turno.IdEstadoTurno,
                        FechaHora = DateTime.Now,
                        Accion = "Cambio de estado",
                        Observacion = $"Estado cambiado de {estadoAnterior?.Nombre} a {estadoNuevo?.Nombre}"
                    });
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurnoExists(turno.IdTurno)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewData["IdNinoAdolescente"] = new SelectList(_context.NinosAdolescentes, "IdNinoAdolescente", "Apellido", turno.IdNinoAdolescente);
        ViewData["IdEstadoTurno"] = new SelectList(_context.EstadosTurno, "IdEstadoTurno", "Nombre", turno.IdEstadoTurno);
        return View(turno);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var turno = await _context.Turnos
            .Include(t => t.NinoAdolescente)
            .Include(t => t.EstadoTurno)
            .FirstOrDefaultAsync(t => t.IdTurno == id);

        if (turno == null) return NotFound();

        return View(turno);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var turno = await _context.Turnos.FindAsync(id);
        if (turno != null)
        {
            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool TurnoExists(int id)
    {
        return _context.Turnos.Any(e => e.IdTurno == id);
    }
}
