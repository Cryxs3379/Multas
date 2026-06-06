// =============================================================================
// Contrato
// =============================================================================
// QUÉ ES: Contrato de alquiler del cliente, obtenido por DNI.
// PROYECTO REAL: Venía de una consulta SQL Server: SELECT ... WHERE Dni = @dni
// BUENA PRÁCTICA: Modelo de dominio independiente de cómo se obtiene (Repository).
// =============================================================================

namespace SimulacionMultas.Models;

public class Contrato
{
    public int Numero { get; set; }
    public string Dni { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Idioma { get; set; } = string.Empty; // ES, EN, FR...
}
