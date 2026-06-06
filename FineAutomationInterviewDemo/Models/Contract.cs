using FineAutomationInterviewDemo.Enums;

namespace FineAutomationInterviewDemo.Models;

public class Contract
{
    public int ContractNumber { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CarPlate { get; set; } = string.Empty;
    public Language Language { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
