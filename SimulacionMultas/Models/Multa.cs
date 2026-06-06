// =============================================================================
// Multa
// =============================================================================
// QUÉ ES: Objeto de negocio con los datos extraídos del PDF (tras regex/parser).
// PROYECTO REAL: Se creaba después de leer el PDF con expresiones regulares.
// BUENA PRÁCTICA: Separar "archivo descargado" (MultaDescargada) de "dato de negocio" (Multa).
// =============================================================================

namespace SimulacionMultas.Models;

public class Multa
{
    public string Dni { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
    public DateTime FechaMulta { get; set; }
    public string ReferenciaInterna { get; set; } = string.Empty; // Generada por el procesador del origen
}
