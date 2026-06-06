namespace FineAutomationInterviewDemo.Services;

public class ConsoleProcessingLogger : IProcessingLogger
{
    public void LogSeparator() => Console.WriteLine("========================================");

    public void LogInfo(string message) => Console.WriteLine(message);

    public void LogError(string message) => Console.WriteLine($"ERROR: {message}");
}
