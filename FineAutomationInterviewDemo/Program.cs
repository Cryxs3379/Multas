using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Infrastructure;
using FineAutomationInterviewDemo.Models;
using FineAutomationInterviewDemo.Repositories;
using FineAutomationInterviewDemo.Services;
using FineAutomationInterviewDemo.Strategies;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
ConfigureServices(services);

using var serviceProvider = services.BuildServiceProvider();

var seeder = serviceProvider.GetRequiredService<FakeFileSystemSeeder>();
seeder.Seed();

var processingService = serviceProvider.GetRequiredService<IFineProcessingService>();
var fines = CreateSampleFines();

Console.WriteLine();
Console.WriteLine("=== FineAutomationInterviewDemo ===");
Console.WriteLine("Demo de automatización de multas para entrevista técnica .NET");
Console.WriteLine();

foreach (var fine in fines)
{
    var result = processingService.Process(fine);

    if (!result.Success)
        Console.WriteLine($"[FALLO] {result.Message}");
}

static void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IContractRepository, FakeContractRepository>();
    services.AddSingleton<IServerFileService, ServerFileService>();
    services.AddSingleton<IOutputFolderService, OutputFolderService>();
    services.AddSingleton<IFineProcessingService, FineProcessingService>();
    services.AddSingleton<FakeFileSystemSeeder>();

    services.AddSingleton<IFineOriginProcessor, MalagaFineProcessor>();
    services.AddSingleton<IFineOriginProcessor, SevillaFineProcessor>();
    services.AddSingleton<IFineOriginProcessor, GranadaFineProcessor>();
    services.AddSingleton<IFineOriginProcessor, DgtFineProcessor>();

    services.AddSingleton<FineOriginProcessorFactory>(sp =>
        new FineOriginProcessorFactory(sp.GetServices<IFineOriginProcessor>()));
}

static List<Fine> CreateSampleFines() =>
[
    new Fine
    {
        Id = Guid.NewGuid(),
        CustomerName = "Juan Pérez García",
        Dni = "12345678A",
        FineDate = new DateTime(2026, 6, 1),
        Origin = FineOrigin.Malaga,
        OriginalFileName = "multa_malaga_juan.txt"
    },
    new Fine
    {
        Id = Guid.NewGuid(),
        CustomerName = "John Smith",
        Dni = "X1234567B",
        FineDate = new DateTime(2026, 6, 2),
        Origin = FineOrigin.DGT,
        OriginalFileName = "multa_dgt_john.txt"
    },
    new Fine
    {
        Id = Guid.NewGuid(),
        CustomerName = "Marie Dupont",
        Dni = "Y7654321C",
        FineDate = new DateTime(2026, 6, 3),
        Origin = FineOrigin.Sevilla,
        OriginalFileName = "multa_sevilla_marie.txt"
    },
    new Fine
    {
        Id = Guid.NewGuid(),
        CustomerName = "Cliente Sin Contrato",
        Dni = "00000000Z",
        FineDate = new DateTime(2026, 6, 4),
        Origin = FineOrigin.Granada,
        OriginalFileName = "multa_granada_error.txt"
    }
];
