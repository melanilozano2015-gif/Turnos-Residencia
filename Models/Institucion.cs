using System.ComponentModel.DataAnnotations;

namespace ResidenciaApp.Models;

public class Institucion
{
    [Key]
    public int IdInstitucion { get; set; }

    [Required]
    [StringLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200)]
    public string Calle { get; set; } = string.Empty;

    [StringLength(10)]
    public string Numero { get; set; } = string.Empty;

    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
