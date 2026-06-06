using FineAutomationInterviewDemo.Models;
using FineAutomationInterviewDemo.Repositories;
using FineAutomationInterviewDemo.Strategies;

namespace FineAutomationInterviewDemo.Services;

public class FineProcessingService : IFineProcessingService
{
    private readonly FineOriginProcessorFactory _processorFactory;
    private readonly IContractRepository _contractRepository;
    private readonly IServerFileService _serverFileService;
    private readonly IOutputFolderService _outputFolderService;

    public FineProcessingService(
        FineOriginProcessorFactory processorFactory,
        IContractRepository contractRepository,
        IServerFileService serverFileService,
        IOutputFolderService outputFolderService)
    {
        _processorFactory = processorFactory;
        _contractRepository = contractRepository;
        _serverFileService = serverFileService;
        _outputFolderService = outputFolderService;
    }

    public ProcessingResult Process(Fine fine)
    {
        LogSeparator();
        Log($"Procesando multa de {fine.CustomerName}");
        Log($"Origen seleccionado: {fine.Origin}");

        var originResult = ProcessByOrigin(fine);
        if (!originResult.Success)
        {
            Log($"ERROR: {originResult.Message}");
            LogSeparator();
            return ProcessingResult.Fail(fine, originResult.Message);
        }

        Log(originResult.Message);
        Log($"Referencia interna generada: {originResult.InternalReference}");

        var contract = FindContract(fine.Dni);
        if (contract is null)
        {
            var message = $"No se encontró contrato para el DNI {fine.Dni}";
            Log($"ERROR: {message}");
            LogSeparator();
            return ProcessingResult.Fail(fine, message);
        }

        Log($"Contrato encontrado: {contract.ContractNumber}");

        if (!IsFineDateWithinContract(fine, contract))
        {
            const string message = "La fecha de la multa no está dentro del periodo del contrato";
            Log($"ERROR: {message}");
            LogSeparator();
            return ProcessingResult.Fail(fine, message);
        }

        var fileError = ValidateServerFiles(fine, contract);
        if (fileError is not null)
        {
            Log($"ERROR: {fileError}");
            LogSeparator();
            return ProcessingResult.Fail(fine, fileError);
        }

        var outputPath = CreateFinalOutput(fine, contract, originResult.InternalReference!);
        Log("Proceso completado correctamente.");
        Log($"Carpeta creada: Output/Processed/{contract.CarPlate}_{contract.ContractNumber}");
        LogSeparator();

        return ProcessingResult.Ok(fine, contract, outputPath, originResult.InternalReference!);
    }

    private OriginProcessingResult ProcessByOrigin(Fine fine)
    {
        try
        {
            var processor = _processorFactory.GetProcessor(fine.Origin);
            return processor.Process(fine);
        }
        catch (ProcessorNotFoundException ex)
        {
            return OriginProcessingResult.Fail(ex.Message);
        }
    }

    private Contract? FindContract(string dni)
    {
        Log("Buscando contrato por DNI...");
        return _contractRepository.GetByDni(dni);
    }

    private static bool IsFineDateWithinContract(Fine fine, Contract contract)
    {
        Console.WriteLine("Validando fechas del contrato...");
        return fine.FineDate.Date >= contract.StartDate.Date
            && fine.FineDate.Date <= contract.EndDate.Date;
    }

    private string? ValidateServerFiles(Fine fine, Contract contract)
    {
        Log("Buscando multa simulada descargada...");
        var finePath = _serverFileService.GetFineFilePath(fine.OriginalFileName);
        if (!_serverFileService.FileExists(finePath))
            return $"No se encontró la multa simulada: {fine.OriginalFileName}";

        Log("Buscando contrato en servidor...");
        var contractPath = _serverFileService.GetContractFilePath(contract.ContractNumber);
        if (!_serverFileService.FileExists(contractPath))
            return $"No se encontró el contrato en servidor: {contract.ContractNumber}";

        Log("Buscando documento de ayuda...");
        var helpPath = _serverFileService.GetHelpDocumentPath(contract.Language);
        if (!_serverFileService.FileExists(helpPath))
            return $"No se encontró el documento de ayuda para idioma: {contract.Language}";

        return null;
    }

    private string CreateFinalOutput(Fine fine, Contract contract, string internalReference)
    {
        Log("Creando carpeta final...");

        var finePath = _serverFileService.GetFineFilePath(fine.OriginalFileName);
        var contractPath = _serverFileService.GetContractFilePath(contract.ContractNumber);
        var helpPath = _serverFileService.GetHelpDocumentPath(contract.Language);

        var outputPath = _outputFolderService.CreateOutputFolder(contract);
        _outputFolderService.CopyFilesToOutput(outputPath, finePath, contractPath, helpPath);
        _outputFolderService.CreateSummaryFile(outputPath, fine, contract, internalReference);

        return outputPath;
    }

    private static void Log(string message) => Console.WriteLine(message);

    private static void LogSeparator() => Console.WriteLine("========================================");
}
