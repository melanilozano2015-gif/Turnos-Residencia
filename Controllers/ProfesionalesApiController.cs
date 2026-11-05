
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Data;

namespace ResidenciaApp.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProfesionalesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProfesionalesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Buscar profesionales por nombre, apellido o matrícula
    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(string? q)
    {
        var query = _context.Profesionales
            .AsNoTracking()
            .AsQueryable();

        // Aplicar filtro si el usuario escribió algo
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();

            query = query.Where(p =>
                p.Nombre.Contains(q) ||
                p.Apellido.Contains(q) ||
                p.Matricula.Contains(q));
        }

        // Devolver solamente los primeros 20 resultados
        var resultados = await query
            .OrderBy(p => p.Apellido)
            .Take(20)
            .Select(p => new
            {
                id = p.IdProfesional,
                text = p.Apellido + ", " + p.Nombre +
                       " (" + p.Matricula + ")"
            })
            .ToListAsync();

        return Ok(resultados);
    }
}

