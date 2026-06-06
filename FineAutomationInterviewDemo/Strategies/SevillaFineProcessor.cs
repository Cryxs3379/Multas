using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Strategies;

public class SevillaFineProcessor : IFineOriginProcessor
{
    public FineOrigin Origin => FineOrigin.Sevilla;

    public OriginProcessingResult Process(Fine fine)
    {
        if (string.IsNullOrWhiteSpace(fine.CustomerName))
            return OriginProcessingResult.Fail("El nombre del cliente es obligatorio para multas de Sevilla.");

        if (string.IsNullOrWhiteSpace(fine.Dni))
            return OriginProcessingResult.Fail("El DNI es obligatorio para multas de Sevilla.");

        var reference = $"SEV-{fine.FineDate:yyyyMMdd}-{fine.Dni}";
        return OriginProcessingResult.Ok("Aplicando reglas específicas de Sevilla", reference);
    }
}
