// =============================================================================
// ContratoRepositoryFake — Repository Pattern
// =============================================================================
// QUÉ ES: Lista en memoria que simula SQL Server.
// PROYECTO REAL: SELECT * FROM Contratos WHERE Dni = @dni
// BUENA PRÁCTICA: Misma interfaz IContratoRepository que una versión SQL futura.
// =============================================================================

using SimulacionMultas.Interfaces;
using SimulacionMultas.Models;

namespace SimulacionMultas.Services;

public class ContratoRepositoryFake : IContratoRepository
{
    private readonly List<Contrato> _contratos =
    [
        new() { Numero = 1001, Dni = "12345678A", Matricula = "1234ABC", Email = "juan@email.com", Idioma = "ES" },
        new() { Numero = 1002, Dni = "X1234567B", Matricula = "5678DEF", Email = "john@email.com", Idioma = "EN" },
        new() { Numero = 1003, Dni = "Y7654321C", Matricula = "9012GHI", Email = "marie@email.com", Idioma = "FR" }
    ];

    public Contrato? BuscarPorDni(string dni) =>
        _contratos.FirstOrDefault(c => c.Dni.Equals(dni, StringComparison.OrdinalIgnoreCase));
}
