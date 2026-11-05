
using ResidenciaApp.Models;

namespace ResidenciaApp.Repositories;

public interface ITurnoRepository
{
	// Obtener todos los turnos con filtros
	Task<List<Turno>> ObtenerTodosAsync(
		string? tab = null,
		string? tipo = null,
		string? search = null,
		int? estadoId = null,
		DateTime? fechaDesde = null,
		DateTime? fechaHasta = null);

	// Obtener un turno por ID
	Task<Turno?> ObtenerPorIdAsync(int id);

	// Obtener turno con relaciones e historial
	Task<Turno?> ObtenerConHistorialAsync(int id);

	// Obtener datos para los desplegables
	Task<List<NinoAdolescente>> ObtenerNinosAsync();
	Task<List<EstadoTurno>> ObtenerEstadosAsync();

	// Registrar un turno y su historial inicial
	Task AgregarAsync(Turno turno);

	// Actualizar turno y registrar cambios de estado
	Task ActualizarAsync(Turno turno);

	// Eliminar un turno
	Task EliminarAsync(int id);

	// Verificar si existe
	Task<bool> ExisteAsync(int id);
}
