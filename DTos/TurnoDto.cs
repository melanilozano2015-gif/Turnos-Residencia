
using System;
using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.DTOs;

public class TurnoDto
{
    public int IdTurno { get; set; }

    [Range(1, int.MaxValue,
        ErrorMessage = "Debés seleccionar un niño, niña o adolescente")]
    public int IdNinoAdolescente { get; set; }

    [Required(ErrorMessage = "El profesional es obligatorio")]
    [StringLength(200,
        ErrorMessage = "El nombre no puede superar los 200 caracteres")]
    public string NombreProfesional { get; set; } = string.Empty;

    [Required(ErrorMessage = "La institución es obligatoria")]
    [StringLength(200,
        ErrorMessage = "La institución no puede superar los 200 caracteres")]
    public string NombreInstitucion { get; set; } = string.Empty;

    [Range(1, int.MaxValue,
        ErrorMessage = "Debés seleccionar un estado")]
    public int IdEstadoTurno { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    [DataType(DataType.Date)]
    public DateTime? Fecha { get; set; }

    [Required(ErrorMessage = "La hora es obligatoria")]
    [DataType(DataType.Time)]
    public DateTime? Hora { get; set; }

    [Required(ErrorMessage = "El tipo de turno es obligatorio")]
    [StringLength(50,
        ErrorMessage = "El tipo de turno no puede superar los 50 caracteres")]
    public string TipoTurno { get; set; } = string.Empty;

    [StringLength(500,
        ErrorMessage = "El motivo no puede superar los 500 caracteres")]
    public string? Motivo { get; set; }

    [StringLength(500,
        ErrorMessage = "El resultado no puede superar los 500 caracteres")]
    public string? Resultado { get; set; }

    [StringLength(1000,
        ErrorMessage = "Las observaciones no pueden superar los 1000 caracteres")]
    public string? Observaciones { get; set; }
}
