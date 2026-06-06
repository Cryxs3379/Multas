namespace FineAutomationInterviewDemo.Models;

public class OriginProcessingResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? InternalReference { get; set; }

    public static OriginProcessingResult Ok(string message, string internalReference) =>
        new() { Success = true, Message = message, InternalReference = internalReference };

    public static OriginProcessingResult Fail(string message) =>
        new() { Success = false, Message = message };
}
