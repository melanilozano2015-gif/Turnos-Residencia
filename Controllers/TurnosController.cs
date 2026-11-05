
using ResidenciaApp.DTOs;
using ResidenciaApp.Repositories;
using ResidenciaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ResidenciaApp.Controllers;

[Authorize]
public class TurnosController : Controller
{
    private readonly ITurnoRepository _repository;

    public TurnosController(ITurnoRepository repository)
    {
        _repository = repository;
    }

    // ==========================================
    // LISTADO DE TURNOS
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> Index(
        string? tab,
        string? tipo,
        string? search,
        int? estadoId,
        DateTime? fechaDesde,
        DateTime? fechaHasta)
    {
        ViewData["Tab"] = tab ?? "Todos";
        ViewData["TipoSeleccionado"] = tipo ?? "";
        ViewData["SearchString"] = search ?? "";

        var estados = await _repository.ObtenerEstadosAsync();

        ViewData["EstadoId"] = new SelectList(
            estados,
            "IdEstadoTurno",
            "Nombre",
            estadoId
        );

        var turnos = await _repository.ObtenerTodosAsync(
            tab,
            tipo,
            search,
            estadoId,
            fechaDesde,
            fechaHasta
        );

        return View(turnos);
    }

    // ==========================================
    // DETALLES DE UN TURNO
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var turno = await _repository
            .ObtenerConHistorialAsync(id.Value);

        if (turno == null)
            return NotFound();

        return View(turno);
    }

    // ==========================================
    // CREAR TURNO - GET
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> Create(int? ninoId)
    {
        var ninos = await _repository.ObtenerNinosAsync();
        var estados = await _repository.ObtenerEstadosAsync();

        ViewData["IdNinoAdolescente"] = new SelectList(
            ninos,
            "IdNinoAdolescente",
            "Apellido",
            ninoId
        );

        ViewData["IdEstadoTurno"] = new SelectList(
            estados,
            "IdEstadoTurno",
            "Nombre"
        );

        var dto = new TurnoDto();

        if (ninoId.HasValue)
        {
            dto.IdNinoAdolescente = ninoId.Value;
        }

        return View(dto);
    }

    // ==========================================
    // CREAR TURNO - POST
    // ==========================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TurnoDto dto)
    {
        if (ModelState.IsValid)
        {
            var turno = new Turno
            {
                IdNinoAdolescente = dto.IdNinoAdolescente,
                NombreProfesional = dto.NombreProfesional,
                NombreInstitucion = dto.NombreInstitucion,
                IdEstadoTurno = dto.IdEstadoTurno,
                TipoTurno = dto.TipoTurno,
                Fecha = dto.Fecha!.Value,
                Hora = dto.Hora!.Value,
                Motivo = dto.Motivo ?? string.Empty,
                Resultado = dto.Resultado ?? string.Empty,
                Observaciones = dto.Observaciones ?? string.Empty
            };

            await _repository.AgregarAsync(turno);

            return RedirectToAction(nameof(Index));
        }

        var ninos = await _repository.ObtenerNinosAsync();
        var estados = await _repository.ObtenerEstadosAsync();

        ViewData["IdNinoAdolescente"] = new SelectList(
            ninos,
            "IdNinoAdolescente",
            "Apellido",
            dto.IdNinoAdolescente
        );

        ViewData["IdEstadoTurno"] = new SelectList(
            estados,
            "IdEstadoTurno",
            "Nombre",
            dto.IdEstadoTurno
        );

        return View(dto);
    }

    // ==========================================
    // EDITAR TURNO - GET
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var turno = await _repository.ObtenerPorIdAsync(id.Value);

        if (turno == null)
            return NotFound();

        var dto = new TurnoDto
        {
            IdTurno = turno.IdTurno,
            IdNinoAdolescente = turno.IdNinoAdolescente,
            NombreProfesional = turno.NombreProfesional,
            NombreInstitucion = turno.NombreInstitucion,
            IdEstadoTurno = turno.IdEstadoTurno,
            TipoTurno = turno.TipoTurno,
            Fecha = turno.Fecha,
            Hora = turno.Hora,
            Motivo = turno.Motivo,
            Resultado = turno.Resultado,
            Observaciones = turno.Observaciones
        };

        var ninos = await _repository.ObtenerNinosAsync();
        var estados = await _repository.ObtenerEstadosAsync();

        ViewData["IdNinoAdolescente"] = new SelectList(
            ninos,
            "IdNinoAdolescente",
            "Apellido",
            dto.IdNinoAdolescente
        );

        ViewData["IdEstadoTurno"] = new SelectList(
            estados,
            "IdEstadoTurno",
            "Nombre",
            dto.IdEstadoTurno
        );

        return View(dto);
    }

    // ==========================================
    // EDITAR TURNO - POST
    // ==========================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TurnoDto dto)
    {
        if (id != dto.IdTurno)
            return NotFound();

        var turno = await _repository.ObtenerPorIdAsync(id);

        if (turno == null)
            return NotFound();

        // Conservar el NNyA asociado originalmente.
        dto.IdNinoAdolescente = turno.IdNinoAdolescente;

        ModelState.Remove(
            nameof(TurnoDto.IdNinoAdolescente)
        );

        if (!ModelState.IsValid)
        {
            var ninos = await _repository.ObtenerNinosAsync();
            var estados = await _repository.ObtenerEstadosAsync();

            ViewData["IdNinoAdolescente"] = new SelectList(
                ninos,
                "IdNinoAdolescente",
                "Apellido",
                dto.IdNinoAdolescente
            );

            ViewData["IdEstadoTurno"] = new SelectList(
                estados,
                "IdEstadoTurno",
                "Nombre",
                dto.IdEstadoTurno
            );

            return View(dto);
        }

        // Actualizar solamente los campos permitidos.
        turno.NombreProfesional = dto.NombreProfesional;
        turno.NombreInstitucion = dto.NombreInstitucion;
        turno.IdEstadoTurno = dto.IdEstadoTurno;
        turno.TipoTurno = dto.TipoTurno;
        turno.Fecha = dto.Fecha!.Value;
        turno.Hora = dto.Hora!.Value;
        turno.Motivo = dto.Motivo ?? string.Empty;
        turno.Resultado = dto.Resultado ?? string.Empty;
        turno.Observaciones = dto.Observaciones ?? string.Empty;

        try
        {
            await _repository.ActualizarAsync(turno);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.ExisteAsync(id))
                return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // ==========================================
    // ELIMINAR TURNO - GET
    // ==========================================

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var turno = await _repository
            .ObtenerConHistorialAsync(id.Value);

        if (turno == null)
            return NotFound();

        return View(turno);
    }

    // ==========================================
    // ELIMINAR TURNO - POST
    // ==========================================

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!await _repository.ExisteAsync(id))
            return NotFound();

        await _repository.EliminarAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
