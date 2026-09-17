using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Data;

namespace ResidenciaApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfesionalesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProfesionalesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(string? q)
    {
        var query = _context.Profesionales
            .Include(p => p.Especialidad)
            .AsQueryable();

        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(p =>
                p.Nombre.Contains(q) ||
                p.Apellido.Contains(q) ||
                p.Matricula.Contains(q));
        }

        var resultados = await query
            .OrderBy(p => p.Apellido)
            .Take(20)
            .Select(p => new
            {
                id = p.IdProfesional,
                text = p.Apellido + ", " + p.Nombre + " (" + p.Matricula + ")"
            })
            .ToListAsync();

        return Ok(resultados);
    }
}
