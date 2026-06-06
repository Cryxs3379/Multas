// =============================================================================
// MultaDescargada
// =============================================================================
// QUÉ ES: Lo que Pyramid devuelve al descargar una multa (antes de leer el PDF).
// PROYECTO REAL: Selenium descargaba el PDF y guardaba el origen (Málaga, DGT...).
// BUENA PRÁCTICA: Modelo simple que representa datos en un paso concreto del flujo.
// =============================================================================

namespace SimulacionMultas.Models;

public class MultaDescargada
{
    public string Origen { get; set; } = string.Empty;       // Ej: "Malaga", "DGT", "Cordoba" (origen desconocido)
    public string RutaPdf { get; set; } = string.Empty;      // Ruta del PDF descargado (simulada)
    public DateTime FechaDescarga { get; set; }              // Fecha en que se procesa el lote
}
