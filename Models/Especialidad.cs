using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.Models;

public class Especialidad
{
    [Key]
    public int IdEspecialidad { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    public ICollection<Profesional> Profesionales { get; set; } = new List<Profesional>();
}
