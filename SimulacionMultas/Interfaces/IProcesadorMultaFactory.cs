// =============================================================================
// IProcesadorMultaFactory
// =============================================================================
// QUÉ ES: Elige el procesador correcto según el origen de la multa.
// PROYECTO REAL: Evitaba un switch gigante en el servicio principal.
// BUENA PRÁCTICA: Si el origen es nuevo → ProcesadorMultaDefault (como en producción).
// =============================================================================

namespace SimulacionMultas.Interfaces;

public interface IProcesadorMultaFactory
{
    IProcesadorMulta ObtenerProcesador(string origen);
}
