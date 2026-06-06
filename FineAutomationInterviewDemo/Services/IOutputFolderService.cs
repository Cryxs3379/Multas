using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Services;

public interface IOutputFolderService
{
    string CreateProcessedPackage(
        Fine fine,
        Contract contract,
        string internalReference,
        ServerFilesBundle serverFiles);
}
