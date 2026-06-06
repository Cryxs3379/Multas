// =============================================================================
// IProcesadorMulta — Strategy Pattern
// =============================================================================
// QUÉ ES: Cada origen (Málaga, DGT, default) implementa reglas distintas.
// PROYECTO REAL: Validaciones y formato de referencia cambiaban por ciudad/DGT.
// BUENA PRÁCTICA: Open/Closed — nuevo origen = nueva clase, sin tocar el orquestador.
// =============================================================================

using SimulacionMultas.Models;

namespace SimulacionMultas.Interfaces;

public interface IProcesadorMulta
{
    // Orígenes que este procesador sabe manejar (ej: "Malaga") o "*" para default.
    bool PuedeProcesar(string origen);

    // Convierte texto del PDF en objeto Multa (aquí iría el regex real).
    Multa CrearMulta(string textoPdf, string origen);
}
