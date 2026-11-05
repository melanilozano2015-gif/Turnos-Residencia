
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Data;

namespace ResidenciaApp.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InstitucionesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InstitucionesApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Buscar instituciones por nombre o calle
    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(string? q)
    {
        var query = _context.Instituciones
            .AsNoTracking()
            .AsQueryable();

        // Aplicar filtro si se ingresó un texto
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();

            query = query.Where(i =>
                i.Nombre.Contains(q) ||
                i.Calle.Contains(q));
        }

        // Devolver los primeros 20 resultados
        var resultados = await query
            .OrderBy(i => i.Nombre)
            .Take(20)
            .Select(i => new
            {
                id = i.IdInstitucion,
                text = i.Nombre
            })
            .ToListAsync();

        return Ok(resultados);
    }
}
