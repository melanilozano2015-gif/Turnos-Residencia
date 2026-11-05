
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResidenciaApp.DTOs;
using ResidenciaApp.Models;
using ResidenciaApp.Repositories;

namespace ResidenciaApp.Controllers;

[Authorize(Roles = "Administrador,Equipo Técnico")]
public class NinosController : Controller
{
    private readonly INinoRepository _repository;

    public NinosController(INinoRepository repository)
    {
        _repository = repository;
    }

    // LISTAR Y BUSCAR
    [HttpGet]
    public async Task<IActionResult> Index(string? searchString)
    {
        ViewData["CurrentFilter"] = searchString;

        var ninos = await _repository.ObtenerTodosAsync(searchString);

        return View(ninos);
    }

    // VER DETALLES
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var nino = await _repository.ObtenerConTurnosAsync(id.Value);

        if (nino == null)
            return NotFound();

        return View(nino);
    }

    // FORMULARIO DE CREACIÓN
    [HttpGet]
    public IActionResult Create()
    {
        return View(new NinoDto());
    }

    // GUARDAR NUEVO REGISTRO
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NinoDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        dto.Dni = dto.Dni.Trim();

        if (await _repository.ExistePorDniAsync(dto.Dni))
        {
            ModelState.AddModelError(nameof(dto.Dni),
                "Ya existe un NNyA registrado con ese DNI.");
            return View(dto);
        }

        var nino = new NinoAdolescente
        {
            Dni = dto.Dni,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            FechaNacimiento = dto.FechaNacimiento!.Value,
            Genero = dto.Genero ?? string.Empty,
            Nacionalidad = dto.Nacionalidad ?? "Argentina",
            LugarNacimiento = dto.LugarNacimiento ?? string.Empty,
            NumeroExpediente = dto.NumeroExpediente ?? string.Empty,
            EstadoActual = dto.EstadoActual,
            Escolaridad = dto.Escolaridad ?? string.Empty,
            Telefono = dto.Telefono ?? string.Empty,
            Email = dto.Email ?? string.Empty,
            ObraSocial = dto.ObraSocial ?? string.Empty,
            Domicilio = dto.Domicilio ?? string.Empty
        };

        try
        {
            await _repository.AgregarAsync(nino);
        }
        catch (DbUpdateException ex) when (EsDuplicadoDni(ex))
        {
            ModelState.AddModelError(nameof(dto.Dni),
                "Ya existe un NNyA registrado con ese DNI.");
            return View(dto);
        }

        return RedirectToAction(nameof(Index));
    }

    // FORMULARIO DE EDICIÓN
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var nino = await _repository.ObtenerPorIdAsync(id.Value);

        if (nino == null)
            return NotFound();

        var dto = new NinoDto
        {
            IdNinoAdolescente = nino.IdNinoAdolescente,
            Dni = nino.Dni,
            Nombre = nino.Nombre,
            Apellido = nino.Apellido,
            FechaNacimiento = nino.FechaNacimiento,
            Genero = nino.Genero,
            Nacionalidad = nino.Nacionalidad,
            LugarNacimiento = nino.LugarNacimiento,
            NumeroExpediente = nino.NumeroExpediente,
            EstadoActual = nino.EstadoActual,
            Escolaridad = nino.Escolaridad,
            Telefono = nino.Telefono,
            Email = nino.Email,
            ObraSocial = nino.ObraSocial,
            Domicilio = nino.Domicilio
        };

        return View(dto);
    }

    // GUARDAR MODIFICACIONES
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NinoDto dto)
    {
        if (id != dto.IdNinoAdolescente)
            return NotFound();

        if (!ModelState.IsValid)
            return View(dto);

        dto.Dni = dto.Dni.Trim();

        if (await _repository.ExistePorDniAsync(dto.Dni, id))
        {
            ModelState.AddModelError(nameof(dto.Dni),
                "Ya existe otro NNyA registrado con ese DNI.");
            return View(dto);
        }

        var nino = await _repository.ObtenerPorIdAsync(id);

        if (nino == null)
            return NotFound();

        nino.Dni = dto.Dni;
        nino.Nombre = dto.Nombre;
        nino.Apellido = dto.Apellido;
        nino.FechaNacimiento = dto.FechaNacimiento!.Value;
        nino.Genero = dto.Genero ?? string.Empty;
        nino.Nacionalidad = dto.Nacionalidad ?? "Argentina";
        nino.LugarNacimiento = dto.LugarNacimiento ?? string.Empty;
        nino.NumeroExpediente = dto.NumeroExpediente ?? string.Empty;
        nino.EstadoActual = dto.EstadoActual;
        nino.Escolaridad = dto.Escolaridad ?? string.Empty;
        nino.Telefono = dto.Telefono ?? string.Empty;
        nino.Email = dto.Email ?? string.Empty;
        nino.ObraSocial = dto.ObraSocial ?? string.Empty;
        nino.Domicilio = dto.Domicilio ?? string.Empty;

        try
        {
            await _repository.ActualizarAsync(nino);
        }
        catch (DbUpdateException ex) when (EsDuplicadoDni(ex))
        {
            ModelState.AddModelError(nameof(dto.Dni),
                "Ya existe otro NNyA registrado con ese DNI.");
            return View(dto);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.ExisteAsync(id))
                return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    private static bool EsDuplicadoDni(DbUpdateException ex)
    {
        return ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlException
            && (sqlException.Number == 2601 || sqlException.Number == 2627)
            && sqlException.Message.Contains("IX_NinosAdolescentes_Dni",
                StringComparison.OrdinalIgnoreCase);
    }

    // CONSULTAR TURNOS DEL NNyA
    [HttpGet]
    public async Task<IActionResult> Turnos(int? id)
    {
        if (id == null)
            return NotFound();

        var nino = await _repository.ObtenerConTurnosAsync(id.Value);

        if (nino == null)
            return NotFound();

        return View(nino);
    }

    // CONFIRMAR ELIMINACIÓN
    // Solo Administrador
    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var nino = await _repository.ObtenerPorIdAsync(id.Value);

        if (nino == null)
            return NotFound();

        return View(nino);
    }

    // BAJA LÓGICA DE NNyA
    // Conserva los turnos y el historial médico.
    [Authorize(Roles = "Administrador")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var nino = await _repository.ObtenerPorIdAsync(id);

        if (nino == null)
        {
            return NotFound();
        }

        await _repository.EliminarAsync(id);

        TempData["Success"] =
            "El registro fue dado de baja correctamente. " +
            "Sus turnos e historial médico se conservaron.";

        return RedirectToAction(nameof(Index));
    }
}
