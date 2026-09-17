using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.Models;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Contrasena { get; set; } = string.Empty;

    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    public ICollection<HistorialTurno> HistorialTurnos { get; set; } = new List<HistorialTurno>();
}
