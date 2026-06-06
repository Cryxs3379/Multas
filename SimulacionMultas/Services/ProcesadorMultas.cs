// =============================================================================
// ProcesadorMultas — ORQUESTADOR PRINCIPAL
// =============================================================================
// QUÉ ES: Coordina el flujo completo del día. NO implementa reglas de origen ni SQL.
// PROYECTO REAL: Era el "cerebro" del batch de multas.
// BUENA PRÁCTICA: Inyección por constructor + Single Responsibility + DIP.
//
// NO HACE:
// - No abre Pyramid (delega en IPyramidService)
// - No lee PDFs (delega en IPdfExtractorService)
// - No decide reglas por ciudad (delega en IProcesadorMultaFactory)
// - No consulta SQL (delega en IContratoRepository)
// - No copia archivos (delega en IFileStorageService)
// =============================================================================

using SimulacionMultas.Interfaces;
using SimulacionMultas.Models;

namespace SimulacionMultas.Services;

public class ProcesadorMultas
{
    private readonly IPyramidService _pyramid;
    private readonly IPdfExtractorService _pdfExtractor;
    private readonly IProcesadorMultaFactory _procesadorFactory;
    private readonly IContratoRepository _contratoRepository;
    private readonly IFileStorageService _fileStorage;
    private readonly ILogger _logger;

    // INYECCIÓN POR CONSTRUCTOR: todas las dependencias entran aquí.
    // .NET (o tú en Program.cs) debe proporcionar cada implementación concreta.
    public ProcesadorMultas(
        IPyramidService pyramid,
        IPdfExtractorService pdfExtractor,
        IProcesadorMultaFactory procesadorFactory,
        IContratoRepository contratoRepository,
        IFileStorageService fileStorage,
        ILogger logger)
    {
        _pyramid = pyramid;
        _pdfExtractor = pdfExtractor;
        _procesadorFactory = procesadorFactory;
        _contratoRepository = contratoRepository;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    // Procesa todas las multas descargadas para una fecha
    public IReadOnlyList<ResultadoProcesamiento> ProcesarMultasDelDia(DateTime fecha)
    {
        _logger.Info($"=== Inicio procesamiento del día {fecha:yyyy-MM-dd} ===");

        // PASO 1: Descargar multas desde Pyramid (simulado)
        var multasDescargadas = _pyramid.DescargarMultas(fecha);
        var resultados = new List<ResultadoProcesamiento>();

        // PASO 2: Recorrer cada multa descargada
        foreach (var descargada in multasDescargadas)
        {
            _logger.Info($"--- Procesando origen: {descargada.Origen} | PDF: {descargada.RutaPdf} ---");

            // PASO 3: Extraer texto del PDF (simulado)
            var textoPdf = _pdfExtractor.ExtraerTexto(descargada.RutaPdf);

            // PASO 4: Obtener procesador según origen (Malaga, DGT o Default)
            var procesador = _procesadorFactory.ObtenerProcesador(descargada.Origen);

            // PASO 5: El procesador crea el objeto Multa (aquí iría el regex real)
            var multa = procesador.CrearMulta(textoPdf, descargada.Origen);
            _logger.Info($"Multa creada: {multa.NombreCliente} | Ref: {multa.ReferenciaInterna}");

            // PASO 6: Buscar contrato por DNI
            var contrato = _contratoRepository.BuscarPorDni(multa.Dni);

            // PASO 7: Sin contrato → error y continuar con la siguiente multa
            if (contrato is null)
            {
                var msg = $"No hay contrato para DNI {multa.Dni}";
                _logger.Error(msg);
                resultados.Add(ResultadoProcesamiento.Error(msg));
                continue; // No detiene el batch; sigue con las demás multas
            }

            // PASO 8: Crear carpeta final en el servidor (simulado)
            var carpeta = _fileStorage.CrearCarpetaFinal(multa, contrato);

            // PASO 9: Registrar éxito
            var exito = $"Carpeta creada: {carpeta}";
            _logger.Info(exito);
            resultados.Add(ResultadoProcesamiento.Ok(multa, exito));
        }

        _logger.Info("=== Fin del procesamiento ===");
        return resultados;
    }
}
