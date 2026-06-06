using FineAutomationInterviewDemo.Enums;

namespace FineAutomationInterviewDemo.Services;

public class ServerFileService : IServerFileService
{
    private readonly string _basePath;

    public ServerFileService(string? basePath = null)
    {
        _basePath = basePath ?? Directory.GetCurrentDirectory();
    }

    public string GetFineFilePath(string originalFileName) =>
        Path.Combine(_basePath, "FakeServer", "DownloadedFines", originalFileName);

    public string GetContractFilePath(int contractNumber) =>
        Path.Combine(_basePath, "FakeServer", "Contracts", contractNumber.ToString(), $"contrato_{contractNumber}.txt");

    public string GetHelpDocumentPath(Language language) =>
        Path.Combine(_basePath, "FakeServer", "HelpDocuments", $"ayuda_{language}.txt");

    public bool FileExists(string filePath) => File.Exists(filePath);
}
