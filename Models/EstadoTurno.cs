using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.Models;

public class EstadoTurno
{
    [Key]
    public int IdEstadoTurno { get; set; }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200)]
    public string Descripcion { get; set; } = string.Empty;

    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    public ICollection<HistorialTurno> HistorialTurnos { get; set; } = new List<HistorialTurno>();
}
