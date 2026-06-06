// =============================================================================
// PdfExtractorServiceFake
// =============================================================================
// QUÉ ES: Simula extraer texto de un PDF (sin leer archivos reales).
// PROYECTO REAL: Librería PDF + texto plano para aplicar regex.
// BUENA PRÁCTICA: El procesador recibe texto, no bytes del PDF — responsabilidades separadas.
// =============================================================================

using SimulacionMultas.Interfaces;

namespace SimulacionMultas.Services;

public class PdfExtractorServiceFake : IPdfExtractorService
{
    public string ExtraerTexto(string rutaPdf)
    {
        // Simula distinto contenido según el nombre del PDF (como si el regex tuviera datos distintos)
        if (rutaPdf.Contains("sin_contrato"))
            return "DNI=99999999Z;Nombre=Cliente Desconocido";

        if (rutaPdf.Contains("dgt"))
            return "DNI=X1234567B;Nombre=John Smith";

        if (rutaPdf.Contains("cordoba"))
            return "DNI=Y7654321C;Nombre=Marie Dupont";

        return "DNI=12345678A;Nombre=Juan Pérez";
    }
}
