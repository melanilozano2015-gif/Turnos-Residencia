using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.Models;

public class Profesional
{
    [Key]
    public int IdProfesional { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Matricula { get; set; } = string.Empty;

    [Required]
    public int IdEspecialidad { get; set; }

    public Especialidad? Especialidad { get; set; }

    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
