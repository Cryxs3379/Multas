namespace FineAutomationInterviewDemo.Models;

public class ValidationResult
{
    public bool IsValid { get; init; }
    public string Message { get; init; } = string.Empty;

    public static ValidationResult Valid() =>
        new() { IsValid = true };

    public static ValidationResult Invalid(string message) =>
        new() { IsValid = false, Message = message };
}
