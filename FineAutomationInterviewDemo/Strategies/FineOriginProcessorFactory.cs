using FineAutomationInterviewDemo.Enums;

namespace FineAutomationInterviewDemo.Strategies;

public class FineOriginProcessorFactory : IFineOriginProcessorFactory
{
    private readonly IReadOnlyDictionary<FineOrigin, IFineOriginProcessor> _processors;

    public FineOriginProcessorFactory(IEnumerable<IFineOriginProcessor> processors)
    {
        _processors = processors.ToDictionary(p => p.Origin);
    }

    public IFineOriginProcessor? TryGetProcessor(FineOrigin origin) =>
        _processors.TryGetValue(origin, out var processor) ? processor : null;
}
