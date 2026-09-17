using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.Models;

public class NinoAdolescente
{
    [Key]
    public int IdNinoAdolescente { get; set; }

    [Required]
    [StringLength(20)]
    public string Dni { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime FechaNacimiento { get; set; }

    [StringLength(20)]
    public string Genero { get; set; } = string.Empty;

    [StringLength(100)]
    public string Nacionalidad { get; set; } = "Argentina";

    [StringLength(200)]
    public string LugarNacimiento { get; set; } = string.Empty;

    [StringLength(50)]
    public string NumeroExpediente { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string EstadoActual { get; set; } = "En residencia";

    [StringLength(200)]
    public string Escolaridad { get; set; } = string.Empty;

    [StringLength(30)]
    public string Telefono { get; set; } = string.Empty;

    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    public string ObraSocial { get; set; } = string.Empty;

    [StringLength(500)]
    public string Domicilio { get; set; } = string.Empty;

    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
