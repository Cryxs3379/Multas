using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Strategies;

public class GranadaFineProcessor : IFineOriginProcessor
{
    public FineOrigin Origin => FineOrigin.Granada;

    public OriginProcessingResult Process(Fine fine)
    {
        if (fine.FineDate.Date > DateTime.Today)
            return OriginProcessingResult.Fail("La fecha de la multa no puede ser futura (regla Granada).");

        var reference = $"GRA-{fine.FineDate:yyyyMM}-{fine.Dni}";
        return OriginProcessingResult.Ok("Aplicando reglas específicas de Granada", reference);
    }
}
