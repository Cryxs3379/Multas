using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Strategies;

public class MalagaFineProcessor : IFineOriginProcessor
{
    public FineOrigin Origin => FineOrigin.Malaga;

    public OriginProcessingResult Process(Fine fine)
    {
        if (string.IsNullOrWhiteSpace(fine.Dni))
            return OriginProcessingResult.Fail("El DNI es obligatorio para multas de Málaga.");

        var reference = $"MAL-{fine.FineDate.Year}-{fine.Dni}";
        return OriginProcessingResult.Ok("Aplicando reglas específicas de Málaga", reference);
    }
}
