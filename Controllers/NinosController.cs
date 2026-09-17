using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.Data;
using ResidenciaApp.Models;

namespace ResidenciaApp.Controllers;

[Authorize]
public class NinosController : Controller
{
    private readonly ApplicationDbContext _context;

    public NinosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;

        var ninos = from n in _context.NinosAdolescentes
                    select n;

        if (!string.IsNullOrEmpty(searchString))
        {
            ninos = ninos.Where(n =>
                n.Nombre.Contains(searchString) ||
                n.Apellido.Contains(searchString) ||
                n.Dni.Contains(searchString));
        }

        return View(await ninos.OrderBy(n => n.Apellido).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var nino = await _context.NinosAdolescentes
            .Include(n => n.Turnos).ThenInclude(t => t.EstadoTurno)
            .FirstOrDefaultAsync(n => n.IdNinoAdolescente == id);

        if (nino == null) return NotFound();

        return View(nino);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Dni,Nombre,Apellido,FechaNacimiento,Genero,Nacionalidad,LugarNacimiento,NumeroExpediente,EstadoActual,Escolaridad,Telefono,Email,ObraSocial,Domicilio")] NinoAdolescente nino)
    {
        if (ModelState.IsValid)
        {
            _context.Add(nino);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(nino);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var nino = await _context.NinosAdolescentes.FindAsync(id);
        if (nino == null) return NotFound();

        return View(nino);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdNinoAdolescente,Dni,Nombre,Apellido,FechaNacimiento,Genero,Nacionalidad,LugarNacimiento,NumeroExpediente,EstadoActual,Escolaridad,Telefono,Email,ObraSocial,Domicilio")] NinoAdolescente nino)
    {
        if (id != nino.IdNinoAdolescente) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(nino);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NinoExists(nino.IdNinoAdolescente)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(nino);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var nino = await _context.NinosAdolescentes
            .FirstOrDefaultAsync(n => n.IdNinoAdolescente == id);

        if (nino == null) return NotFound();

        return View(nino);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var nino = await _context.NinosAdolescentes.FindAsync(id);
        if (nino != null)
        {
            _context.NinosAdolescentes.Remove(nino);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Turnos(int? id)
    {
        if (id == null) return NotFound();

        var nino = await _context.NinosAdolescentes
            .Include(n => n.Turnos)
                .ThenInclude(t => t.EstadoTurno)
            .FirstOrDefaultAsync(n => n.IdNinoAdolescente == id);

        if (nino == null) return NotFound();

        return View(nino);
    }

    private bool NinoExists(int id)
    {
        return _context.NinosAdolescentes.Any(e => e.IdNinoAdolescente == id);
    }
}
