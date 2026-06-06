using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Services;

public interface IOutputFolderService
{
    string CreateOutputFolder(Contract contract);
    void CopyFilesToOutput(string outputFolder, string finePath, string contractPath, string helpPath);
    void CreateSummaryFile(string outputFolder, Fine fine, Contract contract, string internalReference);
}
