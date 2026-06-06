using FineAutomationInterviewDemo.Enums;

namespace FineAutomationInterviewDemo.Models;

public class Fine
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public DateTime FineDate { get; set; }
    public FineOrigin Origin { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
}
