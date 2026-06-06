using FineAutomationInterviewDemo.Enums;
using FineAutomationInterviewDemo.Models;

namespace FineAutomationInterviewDemo.Demo;

/// <summary>
/// Datos de ejemplo para la demo. En producción, las multas vendrían del parser de PDFs.
/// </summary>
public static class SampleFineFactory
{
    public static IReadOnlyList<Fine> Create() =>
    [
        new Fine
        {
            Id = Guid.NewGuid(),
            CustomerName = "Juan Pérez García",
            Dni = "12345678A",
            FineDate = new DateTime(2026, 6, 1),
            Origin = FineOrigin.Malaga,
            OriginalFileName = "multa_malaga_juan.txt"
        },
        new Fine
        {
            Id = Guid.NewGuid(),
            CustomerName = "John Smith",
            Dni = "X1234567B",
            FineDate = new DateTime(2026, 6, 2),
            Origin = FineOrigin.DGT,
            OriginalFileName = "multa_dgt_john.txt"
        },
        new Fine
        {
            Id = Guid.NewGuid(),
            CustomerName = "Marie Dupont",
            Dni = "Y7654321C",
            FineDate = new DateTime(2026, 6, 3),
            Origin = FineOrigin.Sevilla,
            OriginalFileName = "multa_sevilla_marie.txt"
        },
        new Fine
        {
            Id = Guid.NewGuid(),
            CustomerName = "Cliente Sin Contrato",
            Dni = "00000000Z",
            FineDate = new DateTime(2026, 6, 4),
            Origin = FineOrigin.Granada,
            OriginalFileName = "multa_granada_error.txt"
        }
    ];
}
