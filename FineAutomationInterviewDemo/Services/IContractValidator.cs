using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Services;

public interface IContractValidator
{
    ValidationResult ValidateFineDateWithinContract(Fine fine, Contract contract);
}
