namespace FineAutomationInterviewDemo.Models;

public class ProcessingResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Fine Fine { get; set; } = null!;
    public Contract? Contract { get; set; }
    public string? OutputFolderPath { get; set; }
    public string? InternalReference { get; set; }

    public static ProcessingResult Ok(
        Fine fine,
        Contract contract,
        string outputFolderPath,
        string internalReference,
        string message = "Proceso completado correctamente.") =>
        new()
        {
            Success = true,
            Message = message,
            Fine = fine,
            Contract = contract,
            OutputFolderPath = outputFolderPath,
            InternalReference = internalReference
        };

    public static ProcessingResult Fail(Fine fine, string message) =>
        new() { Success = false, Message = message, Fine = fine };
}
