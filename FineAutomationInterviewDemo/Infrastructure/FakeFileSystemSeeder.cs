namespace FineAutomationInterviewDemo.Infrastructure;

/// <summary>
/// Crea automáticamente la estructura de carpetas y archivos simulados del servidor.
/// </summary>
public class FakeFileSystemSeeder
{
    private readonly IAppPaths _paths;

    public FakeFileSystemSeeder(IAppPaths paths)
    {
        _paths = paths;
    }

    public void Seed()
    {
        SeedContracts();
        SeedHelpDocuments();
        SeedDownloadedFines();
        Directory.CreateDirectory(_paths.OutputProcessedPath);
    }

    private void SeedContracts()
    {
        CreateFile(
            Path.Combine("Contracts", "1001", "contrato_1001.txt"),
            """
            CONTRATO DE ALQUILER #1001
            Cliente: Juan Pérez García
            DNI: 12345678A
            Matrícula: 1234ABC
            Nacionalidad: España
            Idioma: ES
            Periodo: 2026-05-25 al 2026-06-10
            """);

        CreateFile(
            Path.Combine("Contracts", "1002", "contrato_1002.txt"),
            """
            RENTAL CONTRACT #1002
            Customer: John Smith
            ID: X1234567B
            Plate: 5678DEF
            Nationality: United Kingdom
            Language: EN
            Period: 2026-06-01 to 2026-06-15
            """);

        CreateFile(
            Path.Combine("Contracts", "1003", "contrato_1003.txt"),
            """
            CONTRAT DE LOCATION #1003
            Client: Marie Dupont
            ID: Y7654321C
            Immatriculation: 9012GHI
            Nationalité: France
            Langue: FR
            Période: 2026-05-28 au 2026-06-08
            """);
    }

    private void SeedHelpDocuments()
    {
        CreateFile(
            Path.Combine("HelpDocuments", "ayuda_ES.txt"),
            "Documento de ayuda en español para el cliente sobre cómo gestionar la multa.");

        CreateFile(
            Path.Combine("HelpDocuments", "ayuda_EN.txt"),
            "Help document in English for the customer on how to handle the fine.");

        CreateFile(
            Path.Combine("HelpDocuments", "ayuda_FR.txt"),
            "Document d'aide en français pour le client sur la gestion de l'amende.");
    }

    private void SeedDownloadedFines()
    {
        CreateFile(
            Path.Combine("DownloadedFines", "multa_malaga_juan.txt"),
            """
            MULTA - ORIGEN: MÁLAGA
            Cliente: Juan Pérez García
            DNI: 12345678A
            Fecha: 2026-06-01
            Matrícula: 1234ABC
            Importe: 90,00 EUR
            """);

        CreateFile(
            Path.Combine("DownloadedFines", "multa_dgt_john.txt"),
            """
            FINE - ORIGIN: DGT
            Customer: John Smith
            ID: X1234567B
            Date: 2026-06-02
            Plate: 5678DEF
            Amount: 200,00 EUR
            """);

        CreateFile(
            Path.Combine("DownloadedFines", "multa_sevilla_marie.txt"),
            """
            MULTA - ORIGEN: SEVILLA
            Cliente: Marie Dupont
            DNI: Y7654321C
            Fecha: 2026-06-03
            Matrícula: 9012GHI
            Importe: 120,00 EUR
            """);

        CreateFile(
            Path.Combine("DownloadedFines", "multa_granada_error.txt"),
            """
            MULTA - ORIGEN: GRANADA
            Cliente: Cliente Sin Contrato
            DNI: 00000000Z
            Fecha: 2026-06-04
            Matrícula: DESCONOCIDA
            Importe: 75,00 EUR
            """);
    }

    private void CreateFile(string relativePath, string content)
    {
        var fullPath = Path.Combine(_paths.FakeServerPath, relativePath);
        var directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        if (!File.Exists(fullPath))
            File.WriteAllText(fullPath, content);
    }
}
