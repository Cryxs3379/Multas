namespace FineAutomationInterviewDemo.Infrastructure;

public class AppPaths : IAppPaths
{
    public AppPaths(string? basePath = null)
    {
        BasePath = basePath ?? Directory.GetCurrentDirectory();
        FakeServerPath = Path.Combine(BasePath, "FakeServer");
        OutputProcessedPath = Path.Combine(BasePath, "Output", "Processed");
    }

    public string BasePath { get; }
    public string FakeServerPath { get; }
    public string OutputProcessedPath { get; }
}
