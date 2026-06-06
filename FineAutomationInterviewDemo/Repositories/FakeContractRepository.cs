using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Repositories;

/// <summary>
/// Simula el resultado de: SELECT * FROM Contracts WHERE Dni = @dni
/// En producción se sustituiría por SqlContractRepository sin cambiar el orquestador.
/// </summary>
public class FakeContractRepository : IContractRepository
{
    // Equivalente a una tabla en memoria. En SQL Server sería una consulta parametrizada.
    private readonly List<Contract> _contracts =
    [
        new()
        {
            ContractNumber = 1001,
            CustomerName = "Juan Pérez García",
            Dni = "12345678A",
            Nationality = "España",
            Email = "juan.perez@email.com",
            CarPlate = "1234ABC",
            Language = Language.ES,
            StartDate = new DateTime(2026, 5, 25),
            EndDate = new DateTime(2026, 6, 10)
        },
        new()
        {
            ContractNumber = 1002,
            CustomerName = "John Smith",
            Dni = "X1234567B",
            Nationality = "Reino Unido",
            Email = "john.smith@email.com",
            CarPlate = "5678DEF",
            Language = Language.EN,
            StartDate = new DateTime(2026, 6, 1),
            EndDate = new DateTime(2026, 6, 15)
        },
        new()
        {
            ContractNumber = 1003,
            CustomerName = "Marie Dupont",
            Dni = "Y7654321C",
            Nationality = "Francia",
            Email = "marie.dupont@email.com",
            CarPlate = "9012GHI",
            Language = Language.FR,
            StartDate = new DateTime(2026, 5, 28),
            EndDate = new DateTime(2026, 6, 8)
        }
    ];

    public Contract? GetByDni(string dni)
    {
        if (string.IsNullOrWhiteSpace(dni))
            return null;

        return _contracts.FirstOrDefault(c => c.Dni.Equals(dni, StringComparison.OrdinalIgnoreCase));
    }
}
