using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Data;

namespace ResidenciaApp.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            TotalNinos = await _context.NinosAdolescentes.CountAsync(),
            TurnosPendientes = await _context.Turnos
                .CountAsync(t => t.EstadoTurno.Nombre == "Pendiente"),
            TurnosHoy = await _context.Turnos
                .CountAsync(t => t.Fecha.Date == DateTime.Today),
            TotalProfesionales = await _context.Profesionales.CountAsync(),
            TotalInstituciones = await _context.Instituciones.CountAsync(),
            UltimosNinos = await _context.NinosAdolescentes
                .OrderByDescending(n => n.FechaNacimiento)
                .Take(5)
                .ToListAsync(),
            ProximosTurnos = await _context.Turnos
                .Include(t => t.NinoAdolescente)
                .Include(t => t.EstadoTurno)
                .Where(t => t.EstadoTurno.Nombre == "Pendiente" && t.Fecha >= DateTime.Now)
                .OrderBy(t => t.Fecha)
                .Take(5)
                .ToListAsync()
        };

        return View(viewModel);
    }
}

public class DashboardViewModel
{
    public int TotalNinos { get; set; }
    public int TurnosPendientes { get; set; }
    public int TurnosHoy { get; set; }
    public int TotalProfesionales { get; set; }
    public int TotalInstituciones { get; set; }
    public List<Models.NinoAdolescente> UltimosNinos { get; set; } = new();
    public List<Models.Turno> ProximosTurnos { get; set; } = new();
}
