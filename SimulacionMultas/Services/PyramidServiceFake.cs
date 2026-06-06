// =============================================================================
// PyramidServiceFake
// =============================================================================
// QUÉ ES: Simula la descarga de multas desde Pyramid (sin Selenium).
// PROYECTO REAL: Selenium abría la web, elegía fecha y descargaba PDFs.
// BUENA PRÁCTICA: Implementación fake detrás de IPyramidService (DIP).
// =============================================================================

using SimulacionMultas.Interfaces;
using SimulacionMultas.Models;

namespace SimulacionMultas.Services;

public class PyramidServiceFake : IPyramidService
{
    public IReadOnlyList<MultaDescargada> DescargarMultas(DateTime fecha)
    {
        // Datos inventados: 3 orígenes conocidos + 1 origen nuevo (para probar el default)
        return
        [
            new MultaDescargada { Origen = "Malaga", RutaPdf = "fake/malaga.pdf", FechaDescarga = fecha },
            new MultaDescargada { Origen = "DGT", RutaPdf = "fake/dgt.pdf", FechaDescarga = fecha },
            new MultaDescargada { Origen = "Cordoba", RutaPdf = "fake/cordoba.pdf", FechaDescarga = fecha },
            new MultaDescargada { Origen = "Malaga", RutaPdf = "fake/malaga_sin_contrato.pdf", FechaDescarga = fecha }
        ];
    }
}
