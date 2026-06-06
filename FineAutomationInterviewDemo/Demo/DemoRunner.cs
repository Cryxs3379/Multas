using FineAutomationInterviewDemo.Infrastructure;
using FineAutomationInterviewDemo.Services;

namespace FineAutomationInterviewDemo.Demo;

/// <summary>
/// Punto de entrada de la demo. Program.cs solo compone dependencias y delega aquí.
/// </summary>
public class DemoRunner
{
    private readonly FakeFileSystemSeeder _seeder;
    private readonly IFineProcessingService _processingService;

    public DemoRunner(FakeFileSystemSeeder seeder, IFineProcessingService processingService)
    {
        _seeder = seeder;
        _processingService = processingService;
    }

    public void Run()
    {
        _seeder.Seed();

        Console.WriteLine();
        Console.WriteLine("=== FineAutomationInterviewDemo ===");
        Console.WriteLine("Demo de automatización de multas para entrevista técnica .NET");
        Console.WriteLine();

        foreach (var fine in SampleFineFactory.Create())
        {
            var result = _processingService.Process(fine);

            if (!result.Success)
                Console.WriteLine($"[FALLO] {result.Message}");
        }
    }
}
