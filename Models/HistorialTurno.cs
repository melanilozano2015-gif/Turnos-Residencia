using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.Models;

public class HistorialTurno
{
    [Key]
    public int IdHistorial { get; set; }

    [Required]
    public int IdTurno { get; set; }

    public Turno? Turno { get; set; }

    public int? IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    [Required]
    public int IdEstadoTurno { get; set; }

    public EstadoTurno? EstadoTurno { get; set; }

    [Required]
    public DateTime FechaHora { get; set; }

    [Required]
    [StringLength(100)]
    public string Accion { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Observacion { get; set; } = string.Empty;
}
