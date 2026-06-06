using FineAutomationInterviewDemo.Demo;
using FineAutomationInterviewDemo.Repositories;
using FineAutomationInterviewDemo.Services;
using FineAutomationInterviewDemo.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace FineAutomationInterviewDemo.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFineAutomationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAppPaths, AppPaths>();
        services.AddSingleton<IContractRepository, FakeContractRepository>();
        services.AddSingleton<IProcessingLogger, ConsoleProcessingLogger>();
        services.AddSingleton<IContractValidator, ContractValidator>();
        services.AddSingleton<IServerFileService, ServerFileService>();
        services.AddSingleton<IOutputFolderService, OutputFolderService>();
        services.AddSingleton<IFineProcessingService, FineProcessingService>();
        services.AddSingleton<FakeFileSystemSeeder>();
        services.AddSingleton<DemoRunner>();

        services.AddSingleton<IFineOriginProcessor, MalagaFineProcessor>();
        services.AddSingleton<IFineOriginProcessor, SevillaFineProcessor>();
        services.AddSingleton<IFineOriginProcessor, GranadaFineProcessor>();
        services.AddSingleton<IFineOriginProcessor, DgtFineProcessor>();

        services.AddSingleton<IFineOriginProcessorFactory>(sp =>
            new FineOriginProcessorFactory(sp.GetServices<IFineOriginProcessor>()));

        return services;
    }
}
