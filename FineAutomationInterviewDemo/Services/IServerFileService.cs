using FineAutomationInterviewDemo.Enums;

namespace FineAutomationInterviewDemo.Services;

public interface IServerFileService
{
    string GetFineFilePath(string originalFileName);
    string GetContractFilePath(int contractNumber);
    string GetHelpDocumentPath(Language language);
    bool FileExists(string filePath);
}
