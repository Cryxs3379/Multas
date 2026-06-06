namespace FineAutomationInterviewDemo.Models;

public class FileResolutionResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public ServerFilesBundle? Files { get; init; }

    public static FileResolutionResult Ok(ServerFilesBundle files) =>
        new() { Success = true, Files = files };

    public static FileResolutionResult Fail(string message) =>
        new() { Success = false, Message = message };
}
