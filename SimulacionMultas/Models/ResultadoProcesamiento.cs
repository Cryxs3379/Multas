// =============================================================================
// ResultadoProcesamiento
// =============================================================================
// QUÉ ES: Resultado de procesar UNA multa (éxito o error con mensaje).
// PROYECTO REAL: Se registraba en logs o se devolvía al llamador del batch.
// BUENA PRÁCTICA: Result object en lugar de lanzar excepciones por fallos esperados.
// =============================================================================

namespace SimulacionMultas.Models;

public class ResultadoProcesamiento
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public Multa? Multa { get; set; }

    public static ResultadoProcesamiento Ok(Multa multa, string mensaje) =>
        new() { Exito = true, Mensaje = mensaje, Multa = multa };

    public static ResultadoProcesamiento Error(string mensaje) =>
        new() { Exito = false, Mensaje = mensaje };
}
