using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Data;

namespace ResidenciaApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstitucionesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InstitucionesApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(string? q)
    {
        var query = _context.Instituciones.AsQueryable();

        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(i =>
                i.Nombre.Contains(q) ||
                i.Calle.Contains(q));
        }

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
