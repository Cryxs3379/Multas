using FineAutomationInterviewDemo.Models;
using FineAutomationInterviewDemo.Repositories;
using FineAutomationInterviewDemo.Strategies;

namespace FineAutomationInterviewDemo.Services;

/// <summary>
/// Orquesta el flujo de procesamiento. No aplica reglas de origen ni accede al disco directamente.
/// </summary>
public class FineProcessingService : IFineProcessingService
{
    private readonly IFineOriginProcessorFactory _processorFactory;
    private readonly IContractRepository _contractRepository;
    private readonly IContractValidator _contractValidator;
    private readonly IServerFileService _serverFileService;
    private readonly IOutputFolderService _outputFolderService;
    private readonly IProcessingLogger _logger;

    public FineProcessingService(
        IFineOriginProcessorFactory processorFactory,
        IContractRepository contractRepository,
        IContractValidator contractValidator,
        IServerFileService serverFileService,
        IOutputFolderService outputFolderService,
        IProcessingLogger logger)
    {
        _processorFactory = processorFactory;
        _contractRepository = contractRepository;
        _contractValidator = contractValidator;
        _serverFileService = serverFileService;
        _outputFolderService = outputFolderService;
        _logger = logger;
    }

    public ProcessingResult Process(Fine fine)
    {
        _logger.LogSeparator();
        _logger.LogInfo($"Procesando multa de {fine.CustomerName}");
        _logger.LogInfo($"Origen seleccionado: {fine.Origin}");

        var originResult = ApplyOriginRules(fine);
        if (!originResult.Success)
            return FailAndClose(fine, originResult.Message);

        _logger.LogInfo(originResult.Message);
        _logger.LogInfo($"Referencia interna generada: {originResult.InternalReference}");

        var contract = ResolveContract(fine.Dni);
        if (contract is null)
            return FailAndClose(fine, $"No se encontró contrato para el DNI {fine.Dni}");

        _logger.LogInfo($"Contrato encontrado: {contract.ContractNumber}");

        var dateValidation = ValidateContractDates(fine, contract);
        if (!dateValidation.IsValid)
            return FailAndClose(fine, dateValidation.Message);

        var filesResult = ResolveServerFiles(fine, contract);
        if (!filesResult.Success)
            return FailAndClose(fine, filesResult.Message);

        var outputPath = BuildOutputPackage(fine, contract, originResult.InternalReference!, filesResult.Files!);
        return CompleteAndClose(fine, contract, outputPath, originResult.InternalReference!);
    }

    private OriginProcessingResult ApplyOriginRules(Fine fine)
    {
        var processor = _processorFactory.TryGetProcessor(fine.Origin);
        if (processor is null)
            return OriginProcessingResult.Fail($"No se encontró procesador para el origen: {fine.Origin}");

        return processor.Process(fine);
    }

    private Contract? ResolveContract(string dni)
    {
        _logger.LogInfo("Buscando contrato por DNI...");
        return _contractRepository.GetByDni(dni);
    }

    private ValidationResult ValidateContractDates(Fine fine, Contract contract)
    {
        _logger.LogInfo("Validando fechas del contrato...");
        return _contractValidator.ValidateFineDateWithinContract(fine, contract);
    }

    private FileResolutionResult ResolveServerFiles(Fine fine, Contract contract) =>
        _serverFileService.TryResolveRequiredFiles(fine, contract);

    private string BuildOutputPackage(
        Fine fine,
        Contract contract,
        string internalReference,
        ServerFilesBundle serverFiles)
    {
        _logger.LogInfo("Creando carpeta final...");
        return _outputFolderService.CreateProcessedPackage(fine, contract, internalReference, serverFiles);
    }

    private ProcessingResult FailAndClose(Fine fine, string message)
    {
        _logger.LogError(message);
        _logger.LogSeparator();
        return ProcessingResult.Fail(fine, message);
    }

    private ProcessingResult CompleteAndClose(
        Fine fine,
        Contract contract,
        string outputPath,
        string internalReference)
    {
        _logger.LogInfo("Proceso completado correctamente.");
        _logger.LogInfo($"Carpeta creada: Output/Processed/{contract.CarPlate}_{contract.ContractNumber}");
        _logger.LogSeparator();

        return ProcessingResult.Ok(fine, contract, outputPath, internalReference);
    }
}
