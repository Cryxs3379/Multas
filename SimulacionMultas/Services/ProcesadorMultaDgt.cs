// =============================================================================
// ProcesadorMultaDgt — Strategy concreta
// =============================================================================
// QUÉ ES: Reglas específicas para multas de la DGT.
// PROYECTO REAL: Formato de referencia DGT-{fecha}-{dni}.
// BUENA PRÁCTICA: Misma interfaz IProcesadorMulta, distinta implementación.
// =============================================================================

using SimulacionMultas.Interfaces;
using SimulacionMultas.Models;

namespace SimulacionMultas.Services;

public class ProcesadorMultaDgt : IProcesadorMulta
{
    public bool PuedeProcesar(string origen) =>
        origen.Equals("DGT", StringComparison.OrdinalIgnoreCase);

    public Multa CrearMulta(string textoPdf, string origen)
    {
        var dni = ExtraerValor(textoPdf, "DNI");
        var nombre = ExtraerValor(textoPdf, "Nombre");

        return new Multa
        {
            Dni = dni,
            NombreCliente = nombre,
            Origen = origen,
            FechaMulta = DateTime.Today,
            ReferenciaInterna = $"DGT-{DateTime.Today:yyyyMMdd}-{dni}"
        };
    }

    private static string ExtraerValor(string texto, string clave)
    {
        var parte = texto.Split(';').FirstOrDefault(p => p.StartsWith(clave + "="));
        return parte?.Split('=')[1] ?? string.Empty;
    }
}
