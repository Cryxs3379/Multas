// =============================================================================
// ConsoleLogger
// =============================================================================
// QUÉ ES: Implementación de ILogger que escribe en consola.
// PROYECTO REAL: Podría ser Serilog, NLog, etc.
// BUENA PRÁCTICA: Intercambiable sin cambiar ProcesadorMultas.
// =============================================================================

using SimulacionMultas.Interfaces;

namespace SimulacionMultas.Services;

public class ConsoleLogger : ILogger
{
    public void Info(string mensaje) => Console.WriteLine($"[INFO] {mensaje}");

    public void Error(string mensaje) => Console.WriteLine($"[ERROR] {mensaje}");
}
