namespace FineAutomationInterviewDemo.Services;

/// <summary>
/// Abstrae el logging del flujo. En producción se sustituiría por ILogger&lt;T&gt;.
/// </summary>
public interface IProcessingLogger
{
    void LogSeparator();
    void LogInfo(string message);
    void LogError(string message);
}
