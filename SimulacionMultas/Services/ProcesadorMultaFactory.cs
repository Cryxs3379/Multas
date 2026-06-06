// =============================================================================
// ProcesadorMultaFactory
// =============================================================================
// QUÉ ES: Devuelve Malaga, DGT o Default según el origen.
// PROYECTO REAL: Evitaba modificar el servicio principal al añadir ciudades.
// BUENA PRÁCTICA: Recibe IEnumerable<IProcesadorMulta> por DI — se registran todos.
// =============================================================================

using SimulacionMultas.Interfaces;

namespace SimulacionMultas.Services;

public class ProcesadorMultaFactory : IProcesadorMultaFactory
{
    private readonly IEnumerable<IProcesadorMulta> _procesadores;
    private readonly IProcesadorMulta _procesadorDefault;

    // DI inyecta TODOS los IProcesadorMulta registrados + el default explícito
    public ProcesadorMultaFactory(
        IEnumerable<IProcesadorMulta> procesadores,
        ProcesadorMultaDefault procesadorDefault)
    {
        _procesadores = procesadores;
        _procesadorDefault = procesadorDefault;
    }

    public IProcesadorMulta ObtenerProcesador(string origen)
    {
        // Busca el primero que reconozca el origen
        var procesador = _procesadores.FirstOrDefault(p => p.PuedeProcesar(origen));

        // Si no hay ninguno (origen nuevo) → default, como en el proyecto real
        return procesador ?? _procesadorDefault;
    }
}
