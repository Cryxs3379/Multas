// =============================================================================
// ILogger (propia de la simulación, no confundir con Microsoft.Extensions.Logging)
// =============================================================================
// QUÉ ES: Registro de mensajes del flujo.
// PROYECTO REAL: Logs en archivo o consola del proceso batch.
// BUENA PRÁCTICA: Desacoplar Console.WriteLine del código de negocio.
// =============================================================================

namespace SimulacionMultas.Interfaces;

public interface ILogger
{
    void Info(string mensaje);
    void Error(string mensaje);
}
