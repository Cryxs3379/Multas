namespace FineAutomationInterviewDemo.Infrastructure;

/// <summary>
/// Centraliza las rutas de la aplicación para evitar depender de GetCurrentDirectory() en varios sitios.
/// Facilita tests con rutas temporales.
/// </summary>
public interface IAppPaths
{
    string BasePath { get; }
    string FakeServerPath { get; }
    string OutputProcessedPath { get; }
}
