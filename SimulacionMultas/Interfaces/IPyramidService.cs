// =============================================================================
// IPyramidService
// =============================================================================
// QUÉ ES: Contrato para descargar multas desde Pyramid (web privada).
// PROYECTO REAL: Selenium + Pyramid.
// BUENA PRÁCTICA: Dependency Inversion — el orquestador no conoce Selenium.
// =============================================================================

using SimulacionMultas.Models;

namespace SimulacionMultas.Interfaces;

public interface IPyramidService
{
    // Devuelve la lista de multas descargadas para una fecha (PDFs + origen).
    IReadOnlyList<MultaDescargada> DescargarMultas(DateTime fecha);
}
