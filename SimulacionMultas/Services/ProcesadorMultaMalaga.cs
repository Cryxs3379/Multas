// =============================================================================
// ProcesadorMultaMalaga — Strategy concreta
// =============================================================================
// QUÉ ES: Reglas específicas para multas de Málaga.
// PROYECTO REAL: Referencia interna formato MAL-{año}-{dni}, validaciones propias.
// BUENA PRÁCTICA: Una clase por origen; el orquestador no tiene if/switch por ciudad.
// =============================================================================

using SimulacionMultas.Interfaces;
using SimulacionMultas.Models;

namespace SimulacionMultas.Services;

public class ProcesadorMultaMalaga : IProcesadorMulta
{
    public bool PuedeProcesar(string origen) =>
        origen.Equals("Malaga", StringComparison.OrdinalIgnoreCase);

    public Multa CrearMulta(string textoPdf, string origen)
    {
        // En producción aquí irían Regex.Match(...) sobre textoPdf
        var dni = ExtraerValor(textoPdf, "DNI");
        var nombre = ExtraerValor(textoPdf, "Nombre");

        return new Multa
        {
            Dni = dni,
            NombreCliente = nombre,
            Origen = origen,
            FechaMulta = DateTime.Today,
            ReferenciaInterna = $"MAL-{DateTime.Today.Year}-{dni}"
        };
    }

    // Simulación simple de regex: busca "Clave=Valor" en el texto fake
    private static string ExtraerValor(string texto, string clave)
    {
        var parte = texto.Split(';').FirstOrDefault(p => p.StartsWith(clave + "="));
        return parte?.Split('=')[1] ?? string.Empty;
    }
}
