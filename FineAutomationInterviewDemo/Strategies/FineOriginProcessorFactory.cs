using FineAutomationInterviewDemo.Enums;

namespace FineAutomationInterviewDemo.Strategies;

public class FineOriginProcessorFactory
{
    private readonly Dictionary<FineOrigin, IFineOriginProcessor> _processors;

    public FineOriginProcessorFactory(IEnumerable<IFineOriginProcessor> processors)
    {
        _processors = processors.ToDictionary(p => p.Origin);
    }

    public IFineOriginProcessor GetProcessor(FineOrigin origin)
    {
        if (_processors.TryGetValue(origin, out var processor))
            return processor;

        throw new ProcessorNotFoundException(origin);
    }
}
