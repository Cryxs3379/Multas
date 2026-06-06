// =============================================================================
// IPdfExtractorService
// =============================================================================
// QUÉ ES: Contrato para extraer texto de un PDF (luego regex en el procesador).
// PROYECTO REAL: Librería de PDFs + regex sobre el texto.
// BUENA PRÁCTICA: Una responsabilidad — solo "sacar texto", no reglas de negocio.
// =============================================================================

namespace SimulacionMultas.Interfaces;

public interface IPdfExtractorService
{
    string ExtraerTexto(string rutaPdf);
}
