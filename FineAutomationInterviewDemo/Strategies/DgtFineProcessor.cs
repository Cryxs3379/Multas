using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Strategies;

public class DgtFineProcessor : IFineOriginProcessor
{
    public FineOrigin Origin => FineOrigin.DGT;

    public OriginProcessingResult Process(Fine fine)
    {
        if (string.IsNullOrWhiteSpace(fine.Dni))
            return OriginProcessingResult.Fail("El DNI es obligatorio para multas de DGT.");

        if (fine.FineDate == default || fine.FineDate.Date > DateTime.Today)
            return OriginProcessingResult.Fail("La fecha de la multa no es válida (regla DGT).");

        var reference = $"DGT-{fine.FineDate:yyyyMMdd}-{fine.Dni}";
        return OriginProcessingResult.Ok("Aplicando reglas específicas de DGT", reference);
    }
}
