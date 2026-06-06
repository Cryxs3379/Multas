using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Services;

public class OutputFolderService : IOutputFolderService
{
    private readonly string _basePath;

    public OutputFolderService(string? basePath = null)
    {
        _basePath = basePath ?? Directory.GetCurrentDirectory();
    }

    public string CreateOutputFolder(Contract contract)
    {
        var folderName = $"{contract.CarPlate}_{contract.ContractNumber}";
        var outputPath = Path.Combine(_basePath, "Output", "Processed", folderName);
        Directory.CreateDirectory(outputPath);
        return outputPath;
    }

    public void CopyFilesToOutput(string outputFolder, string finePath, string contractPath, string helpPath)
    {
        File.Copy(finePath, Path.Combine(outputFolder, Path.GetFileName(finePath)), overwrite: true);
        File.Copy(contractPath, Path.Combine(outputFolder, Path.GetFileName(contractPath)), overwrite: true);
        File.Copy(helpPath, Path.Combine(outputFolder, Path.GetFileName(helpPath)), overwrite: true);
    }

    public void CreateSummaryFile(string outputFolder, Fine fine, Contract contract, string internalReference)
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
