// =============================================================================
// IContratoRepository — Repository Pattern
// =============================================================================
// QUÉ ES: Abstrae el acceso a contratos (hoy lista fake, mañana SQL Server).
// PROYECTO REAL: Consulta SQL por DNI del cliente de la multa.
// BUENA PRÁCTICA: El orquestador no sabe si los datos vienen de SQL, API o memoria.
// =============================================================================

using SimulacionMultas.Models;

namespace SimulacionMultas.Interfaces;

public interface IContratoRepository
{
    Contrato? BuscarPorDni(string dni);
}
