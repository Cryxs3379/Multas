using FineAutomationInterviewDemo.Infrastructure;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Services;

public class OutputFolderService : IOutputFolderService
{
    private readonly IAppPaths _paths;

    public OutputFolderService(IAppPaths paths)
    {
        _paths = paths;
    }

    public string CreateProcessedPackage(
        Fine fine,
        Contract contract,
        string internalReference,
        ServerFilesBundle serverFiles)
    {
        var outputPath = CreateOutputFolder(contract);
        CopyFiles(outputPath, serverFiles);
        CreateSummaryFile(outputPath, fine, contract, internalReference);
        return outputPath;
    }

    private string CreateOutputFolder(Contract contract)
    {
        var folderName = $"{contract.CarPlate}_{contract.ContractNumber}";
        var outputPath = Path.Combine(_paths.OutputProcessedPath, folderName);
        Directory.CreateDirectory(outputPath);
        return outputPath;
    }

    private static void CopyFiles(string outputFolder, ServerFilesBundle serverFiles)
    {
        File.Copy(serverFiles.FinePath, Path.Combine(outputFolder, Path.GetFileName(serverFiles.FinePath)), overwrite: true);
        File.Copy(serverFiles.ContractPath, Path.Combine(outputFolder, Path.GetFileName(serverFiles.ContractPath)), overwrite: true);
        File.Copy(serverFiles.HelpPath, Path.Combine(outputFolder, Path.GetFileName(serverFiles.HelpPath)), overwrite: true);
    }

    private static void CreateSummaryFile(string outputFolder, Fine fine, Contract contract, string internalReference)
    {
        var summaryPath = Path.Combine(outputFolder, "resumen.txt");
        var content = $"""
            === RESUMEN DE PROCESAMIENTO ===

            Cliente: {fine.CustomerName}
            DNI: {fine.Dni}
            Fecha de multa: {fine.FineDate:yyyy-MM-dd}
            Origen: {fine.Origin}
            Referencia interna: {internalReference}
            Número de contrato: {contract.ContractNumber}
            Matrícula: {contract.CarPlate}
            Email: {contract.Email}
            Idioma: {contract.Language}
            Resultado: Proceso completado correctamente.
            """;

        File.WriteAllText(summaryPath, content);
    }
}
