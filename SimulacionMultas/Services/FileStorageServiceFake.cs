// =============================================================================
// FileStorageServiceFake
// =============================================================================
// QUÉ ES: Simula crear la carpeta final en el servidor de archivos.
// PROYECTO REAL: Copiaba multa + contrato físico + ayuda por idioma a una ruta UNC.
// BUENA PRÁCTICA: Devuelve la ruta creada; no mezcla con lógica de negocio.
// =============================================================================

using SimulacionMultas.Interfaces;
using SimulacionMultas.Models;

namespace SimulacionMultas.Services;

public class FileStorageServiceFake : IFileStorageService
{
    public string CrearCarpetaFinal(Multa multa, Contrato contrato)
    {
        // Simula: Output/{matricula}_{contrato}/
        var ruta = $"Output/{contrato.Matricula}_{contrato.Numero}";
        // En la simulación no creamos carpetas reales; solo devolvemos la ruta
        return ruta;
    }
}
