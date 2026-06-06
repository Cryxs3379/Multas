using FineAutomationInterviewDemo.Demo;
using FineAutomationInterviewDemo.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddFineAutomationServices();

using var serviceProvider = services.BuildServiceProvider();
serviceProvider.GetRequiredService<DemoRunner>().Run();
