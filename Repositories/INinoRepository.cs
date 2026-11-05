
using ResidenciaApp.Models;

namespace ResidenciaApp.Repositories;

public interface INinoRepository
{
    Task<List<NinoAdolescente>> ObtenerTodosAsync(
        string? busqueda = null);

    Task<NinoAdolescente?> ObtenerPorIdAsync(int id);

    Task<NinoAdolescente?> ObtenerConTurnosAsync(int id);

    Task<bool> ExistePorDniAsync(string dni, int? excluirId = null);

    Task AgregarAsync(NinoAdolescente nino);

    Task ActualizarAsync(NinoAdolescente nino);

    Task EliminarAsync(int id);

    Task<bool> ExisteAsync(int id);
}
