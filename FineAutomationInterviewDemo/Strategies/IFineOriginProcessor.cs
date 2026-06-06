using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Strategies;

/// <summary>
/// Strategy Pattern: cada origen encapsula sus validaciones y formato de referencia interna.
/// </summary>
public interface IFineOriginProcessor
{
    FineOrigin Origin { get; }
    OriginProcessingResult Process(Fine fine);
}
