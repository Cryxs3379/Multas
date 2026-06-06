// =============================================================================
// IFileStorageService
// =============================================================================
// QUÉ ES: Crea la carpeta final con multa, contrato y ayudas en el servidor.
// PROYECTO REAL: Copiaba archivos a una ruta de red (UNC).
// BUENA PRÁCTICA: Toda la lógica de archivos fuera del orquestador.
// =============================================================================

using SimulacionMultas.Models;

namespace SimulacionMultas.Interfaces;

public interface IFileStorageService
{
    string CrearCarpetaFinal(Multa multa, Contrato contrato);
}
