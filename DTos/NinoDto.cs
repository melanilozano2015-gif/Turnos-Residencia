
using System;
using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.DTOs;

public class NinoDto
{
    public int IdNinoAdolescente { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(20, ErrorMessage = "El DNI no puede superar los 20 caracteres")]
    public string Dni { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    [DataType(DataType.Date)]
    public DateTime? FechaNacimiento { get; set; }

    [StringLength(20, ErrorMessage = "El género no puede superar los 20 caracteres")]
    public string? Genero { get; set; }

    [StringLength(100, ErrorMessage = "La nacionalidad no puede superar los 100 caracteres")]
    public string? Nacionalidad { get; set; } = "Argentina";

    [StringLength(200, ErrorMessage = "El lugar de nacimiento no puede superar los 200 caracteres")]
    public string? LugarNacimiento { get; set; }

    [StringLength(50, ErrorMessage = "El número de expediente no puede superar los 50 caracteres")]
    public string? NumeroExpediente { get; set; }

    [Required(ErrorMessage = "El estado actual es obligatorio")]
    [StringLength(50, ErrorMessage = "El estado actual no puede superar los 50 caracteres")]
    public string EstadoActual { get; set; } = "En residencia";

    [StringLength(200, ErrorMessage = "La escolaridad no puede superar los 200 caracteres")]
    public string? Escolaridad { get; set; }

    [StringLength(30, ErrorMessage = "El teléfono no puede superar los 30 caracteres")]
    public string? Telefono { get; set; }

    [StringLength(100, ErrorMessage = "El email no puede superar los 100 caracteres")]
    [EmailAddress(ErrorMessage = "Ingresá un correo electrónico válido")]
    public string? Email { get; set; }

    [StringLength(100, ErrorMessage = "La obra social no puede superar los 100 caracteres")]
    public string? ObraSocial { get; set; }

    [StringLength(500, ErrorMessage = "El domicilio no puede superar los 500 caracteres")]
    public string? Domicilio { get; set; }
}
