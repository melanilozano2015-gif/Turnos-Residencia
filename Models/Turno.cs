using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.Models;

public class Turno
{
    [Key]
    public int IdTurno { get; set; }

    [Required]
    public int IdNinoAdolescente { get; set; }

    public NinoAdolescente? NinoAdolescente { get; set; }

    [Required]
    [StringLength(200)]
    public string NombreProfesional { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string NombreInstitucion { get; set; } = string.Empty;

    [Required]
    public int IdEstadoTurno { get; set; }

    public EstadoTurno? EstadoTurno { get; set; }

    public int? IdUsuario { get; set; }

    public Usuario? Usuario { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Fecha { get; set; }

    [Required]
    [DataType(DataType.Time)]
    public DateTime Hora { get; set; }

    [Required]
    [StringLength(50)]
    public string TipoTurno { get; set; } = string.Empty;

    [StringLength(500)]
    public string Motivo { get; set; } = string.Empty;

    [StringLength(500)]
    public string Resultado { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Observaciones { get; set; } = string.Empty;

    public ICollection<HistorialTurno> HistorialTurnos { get; set; } = new List<HistorialTurno>();
}
