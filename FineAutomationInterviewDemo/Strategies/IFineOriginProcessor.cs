using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Strategies;

public interface IFineOriginProcessor
{
    FineOrigin Origin { get; }
    OriginProcessingResult Process(Fine fine);
}
