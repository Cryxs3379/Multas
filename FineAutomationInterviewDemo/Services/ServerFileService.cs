using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Infrastructure;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Services;

public class ServerFileService : IServerFileService
{
    private readonly IAppPaths _paths;
    private readonly IProcessingLogger _logger;

    public ServerFileService(IAppPaths paths, IProcessingLogger logger)
    {
        _paths = paths;
        _logger = logger;
    }

    public string GetFineFilePath(string originalFileName) =>
        Path.Combine(_paths.FakeServerPath, "DownloadedFines", originalFileName);

    public string GetContractFilePath(int contractNumber) =>
        Path.Combine(_paths.FakeServerPath, "Contracts", contractNumber.ToString(), $"contrato_{contractNumber}.txt");

    public string GetHelpDocumentPath(Language language) =>
        Path.Combine(_paths.FakeServerPath, "HelpDocuments", $"ayuda_{language}.txt");

    public bool FileExists(string filePath) => File.Exists(filePath);

    public FileResolutionResult TryResolveRequiredFiles(Fine fine, Contract contract)
    {
        _logger.LogInfo("Buscando multa simulada descargada...");
        var finePath = GetFineFilePath(fine.OriginalFileName);
        if (!FileExists(finePath))
            return FileResolutionResult.Fail($"No se encontró la multa simulada: {fine.OriginalFileName}");

        _logger.LogInfo("Buscando contrato en servidor...");
        var contractPath = GetContractFilePath(contract.ContractNumber);
        if (!FileExists(contractPath))
            return FileResolutionResult.Fail($"No se encontró el contrato en servidor: {contract.ContractNumber}");

        _logger.LogInfo("Buscando documento de ayuda...");
        var helpPath = GetHelpDocumentPath(contract.Language);
        if (!FileExists(helpPath))
            return FileResolutionResult.Fail($"No se encontró el documento de ayuda para idioma: {contract.Language}");

        return FileResolutionResult.Ok(new ServerFilesBundle
        {
            FinePath = finePath,
            ContractPath = contractPath,
            HelpPath = helpPath
        });
    }
}
