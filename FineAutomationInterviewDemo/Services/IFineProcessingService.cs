using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Services;

public interface IFineProcessingService
{
    ProcessingResult Process(Fine fine);
}
