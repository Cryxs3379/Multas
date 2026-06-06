using FineAutomationInterviewDemo.Enums;

namespace FineAutomationInterviewDemo.Strategies;

public class ProcessorNotFoundException(FineOrigin origin)
    : Exception($"No se encontró procesador para el origen: {origin}")
{
    public FineOrigin Origin { get; } = origin;
}
