using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Repositories;

/// <summary>
/// Abstracción del acceso a contratos. Permite sustituir FakeContractRepository por SqlContractRepository.
/// </summary>
public interface IContractRepository
{
    Contract? GetByDni(string dni);
}
