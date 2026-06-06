// =============================================================================
// ProcesadorMultaDefault — Strategy de respaldo
// =============================================================================
// QUÉ ES: Procesa orígenes nuevos o no reconocidos (ej: Córdoba apareció un día).
// PROYECTO REAL: Había un procesador genérico para no romper el batch entero.
// BUENA PRÁCTICA: Comportamiento por defecto explícito, no un null ni una excepción.
// =============================================================================

using SimulacionMultas.Interfaces;
using SimulacionMultas.Models;

namespace SimulacionMultas.Services;

public class ProcesadorMultaDefault : IProcesadorMulta
{
    // El default NO se asigna por PuedeProcesar en la factory; es el fallback.
    public bool PuedeProcesar(string origen) => false;

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
            ReferenciaInterna = $"GEN-{origen.ToUpperInvariant()}-{dni}"
        };
    }

    private static string ExtraerValor(string texto, string clave)
    {
        var parte = texto.Split(';').FirstOrDefault(p => p.StartsWith(clave + "="));
        return parte?.Split('=')[1] ?? string.Empty;
    }
}
