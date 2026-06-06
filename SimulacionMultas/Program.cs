// =============================================================================
// Program.cs — Composition Root (composición de dependencias)
// =============================================================================
// QUÉ ES: Punto de entrada. Aquí se "cablea" quién implementa cada interfaz.
// NO contiene lógica de negocio de multas.
//
// USAMOS Microsoft.Extensions.DependencyInjection:
// - ServiceCollection: registro de servicios
// - BuildServiceProvider: crea el contenedor
// - GetRequiredService: pide una instancia ya con todas sus dependencias resueltas
//
// ALTERNATIVA DIDÁCTICA (sin DI container): ver comentario al final del archivo.
// =============================================================================

using Microsoft.Extensions.DependencyInjection;
using SimulacionMultas.Interfaces;
using SimulacionMultas.Services;

Console.WriteLine("Simulación didáctica — Automatización de multas");
Console.WriteLine();

// --- 1. Crear el contenedor de servicios ---
var services = new ServiceCollection();

// --- 2. Registrar cada INTERFAZ → IMPLEMENTACIÓN concreta ---

// Servicios fake que simulan infraestructura externa
services.AddSingleton<ILogger, ConsoleLogger>();
services.AddSingleton<IPyramidService, PyramidServiceFake>();
services.AddSingleton<IPdfExtractorService, PdfExtractorServiceFake>();
services.AddSingleton<IContratoRepository, ContratoRepositoryFake>();
services.AddSingleton<IFileStorageService, FileStorageServiceFake>();

// Strategy: registrar cada procesador de origen
services.AddSingleton<IProcesadorMulta, ProcesadorMultaMalaga>();
services.AddSingleton<IProcesadorMulta, ProcesadorMultaDgt>();
services.AddSingleton<ProcesadorMultaDefault>(); // También como clase concreta para la factory

// Factory: recibe todos los IProcesadorMulta + el default
services.AddSingleton<IProcesadorMultaFactory, ProcesadorMultaFactory>();

// Orquestador principal (recibirá todo por constructor automáticamente)
services.AddSingleton<ProcesadorMultas>();

// --- 3. Construir el proveedor y ejecutar ---
using var provider = services.BuildServiceProvider();

// El contenedor crea ProcesadorMultas e inyecta las 6 dependencias en su constructor
var procesador = provider.GetRequiredService<ProcesadorMultas>();

var resultados = procesador.ProcesarMultasDelDia(new DateTime(2026, 6, 1));

// Resumen final
Console.WriteLine();
Console.WriteLine("--- Resumen ---");
Console.WriteLine($"Total: {resultados.Count} | Éxitos: {resultados.Count(r => r.Exito)} | Errores: {resultados.Count(r => !r.Exito)}");

/*
 * =============================================================================
 * ALTERNATIVA SIN DI CONTAINER (para entender la inyección manual):
 * =============================================================================
 *
 * var logger = new ConsoleLogger();
 * var pyramid = new PyramidServiceFake();
 * var pdf = new PdfExtractorServiceFake();
 * var repo = new ContratoRepositoryFake();
 * var storage = new FileStorageServiceFake();
 * var defaultProc = new ProcesadorMultaDefault();
 * var factory = new ProcesadorMultaFactory(
 *     new IProcesadorMulta[] { new ProcesadorMultaMalaga(), new ProcesadorMultaDgt() },
 *     defaultProc);
 *
 * var procesador = new ProcesadorMultas(pyramid, pdf, factory, repo, storage, logger);
 * procesador.ProcesarMultasDelDia(DateTime.Today);
 *
 * Es lo mismo que hace DI, pero tú conectas las piezas a mano.
 * =============================================================================
 */
