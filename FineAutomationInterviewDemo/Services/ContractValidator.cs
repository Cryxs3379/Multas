using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Services;

public class ContractValidator : IContractValidator
{
    public ValidationResult ValidateFineDateWithinContract(Fine fine, Contract contract)
    {
        var isWithinRange = fine.FineDate.Date >= contract.StartDate.Date
            && fine.FineDate.Date <= contract.EndDate.Date;

        return isWithinRange
            ? ValidationResult.Valid()
            : ValidationResult.Invalid("La fecha de la multa no está dentro del periodo del contrato");
    }
}
