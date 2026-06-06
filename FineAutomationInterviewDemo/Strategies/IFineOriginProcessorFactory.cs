using FineAutomationInterviewDemo.Enums;

namespace FineAutomationInterviewDemo.Strategies;

public interface IFineOriginProcessorFactory
{
    IFineOriginProcessor? TryGetProcessor(FineOrigin origin);
}
