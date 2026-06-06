# FineAutomationInterviewDemo

Demo técnica en **C# / .NET 8** para practicar y demostrar buenas prácticas en una entrevista técnica .NET.

Basada en un caso real de automatización de multas para una empresa de rent a car, pero **simplificada y anonimizada** para poder explicarla sin dependencias externas ni datos privados.

---

## Guión de 2 minutos (para entrevista)

> Automatizaba multas de rent a car. En producción, Selenium descargaba PDFs desde Pyramid, se parseaban con regex y se consultaba SQL Server. Para la demo eliminé todo eso y empiezo cuando la multa ya está parseada como objeto `Fine`.
>
> El orquestador `FineProcessingService` no decide reglas por origen: delega en Strategy Pattern. Cada ciudad/DGT tiene su procesador. Luego busca el contrato vía Repository (hoy fake, mañana SQL), valida fechas, resuelve archivos del servidor simulado y genera la carpeta final.
>
> `Program.cs` solo compone dependencias. La demo arranca en `DemoRunner`. Si mañana añado Córdoba, creo un procesador y lo registro en DI: el orquestador no cambia.

---

## 1. ¿Qué problema resuelve?

Automatiza el procesamiento posterior a la descarga de una multa:

1. Aplica reglas de negocio según el **origen** (Málaga, Sevilla, Granada, DGT).
2. Busca el **contrato** del cliente por DNI.
3. Valida que la fecha de la multa esté dentro del periodo del contrato.
4. Recupera archivos del **servidor simulado** (multa, contrato, ayuda por idioma).
5. Genera una **carpeta final** con toda la documentación.

---

## 2. Arquitectura del proyecto

```
FineAutomationInterviewDemo/
 ├── Program.cs                         # Solo composición DI (9 líneas)
 ├── Demo/
 │   ├── DemoRunner.cs                  # Ejecuta la demo
 │   └── SampleFineFactory.cs           # Multas de ejemplo
 ├── Infrastructure/
 │   ├── ServiceCollectionExtensions.cs # Registro de dependencias
 │   ├── IAppPaths / AppPaths           # Rutas centralizadas
 │   └── FakeFileSystemSeeder.cs        # Datos simulados
 ├── Models/                            # Entidades y resultados
 ├── Enums/                             # FineOrigin, Language
 ├── Repositories/                      # Repository Pattern
 ├── Services/                          # Orquestación y colaboradores
 └── Strategies/                        # Strategy Pattern por origen
```

### Mapa de responsabilidades

| Clase / Interfaz | Responsabilidad única |
|---|---|
| `Program.cs` | Componer el contenedor DI y arrancar |
| `DemoRunner` | Sembrar datos y ejecutar casos de ejemplo |
| `FineProcessingService` | Orquestar el flujo paso a paso |
| `IFineOriginProcessor` | Reglas y referencia interna por origen |
| `IFineOriginProcessorFactory` | Resolver el procesador sin switch |
| `IContractRepository` | Acceso a contratos (abstracción de datos) |
| `IContractValidator` | Validar fechas multa vs contrato |
| `IServerFileService` | Rutas y existencia de archivos del servidor |
| `IOutputFolderService` | Carpeta final, copias y resumen |
| `IProcessingLogger` | Logging desacoplado (hoy consola) |
| `IAppPaths` | Rutas base para testabilidad |

---

## 3. Flujo de ejecución

```
Fine
  │
  ▼
FineProcessingService.Process()
  │
  ├─► ApplyOriginRules()        → IFineOriginProcessorFactory → Strategy
  ├─► ResolveContract()         → IContractRepository
  ├─► ValidateContractDates()   → IContractValidator
  ├─► ResolveServerFiles()      → IServerFileService
  └─► BuildOutputPackage()      → IOutputFolderService
        │
        ▼
  ProcessingResult (éxito o fallo controlado)
```

Los errores de negocio **no lanzan excepciones**: devuelven `ProcessingResult.Fail` con mensaje claro.

---

## 4. Decisiones de diseño conscientes

### ¿Por qué `Program.cs` es tan pequeño?

Porque su único trabajo es **composición raíz** (Composition Root). Toda la lógica de demo vive en `DemoRunner` y los datos de ejemplo en `SampleFineFactory`. En una entrevista puedes decir: *"Program no tiene lógica de negocio; solo cablea dependencias."*

### ¿Por qué `FineProcessingService` no hace todo?

Antes mezclaba logging, validación de fechas, comprobación de archivos y copias. Ahora **solo orquesta** y delega:

- Reglas de origen → Strategy
- Contratos → Repository
- Fechas → `IContractValidator`
- Archivos → `IServerFileService`
- Salida → `IOutputFolderService`
- Logs → `IProcessingLogger`

Eso demuestra **Single Responsibility** sin caer en sobre-arquitectura.

### ¿Por qué result objects en lugar de excepciones?

`OriginProcessingResult`, `ValidationResult`, `FileResolutionResult` y `ProcessingResult` representan **fallos esperados de negocio** (DNI sin contrato, fecha fuera de rango). Las excepciones quedan para errores técnicos reales. Es un criterio defendible en entrevista.

### ¿Por qué `IAppPaths`?

Evita llamar a `Directory.GetCurrentDirectory()` en cinco sitios distintos. En tests puedes inyectar una ruta temporal sin tocar el disco del proyecto.

### ¿Qué NO hice (a propósito)?

- No añadí capas Clean Architecture completas (Application/Domain/Infrastructure separados en proyectos).
- No usé MediatR, AutoMapper ni librerías extra.
- No creé proyecto de tests (documentado cómo hacerlo).

La demo es **pequeña pero profesional**: suficiente para explicar patrones sin perder 10 minutos de la entrevista en estructura.

---

## 5. Patrones utilizados

### Strategy Pattern
`IFineOriginProcessor` + 4 implementaciones. `FineOriginProcessorFactory` resuelve el procesador con un diccionario. **Sin switch en el orquestador.**

### Repository Pattern
`IContractRepository` abstrae el acceso a datos. `FakeContractRepository` simula:

```sql
SELECT * FROM Contracts WHERE Dni = @dni
```

Sustituir por SQL Server = nueva clase + cambiar una línea en DI.

### Dependency Injection
Registro centralizado en `ServiceCollectionExtensions.AddFineAutomationServices()`.

### Separación de responsabilidades
Cada interfaz tiene un motivo claro de cambio (archivos, contratos, logging, validación, salida).

---

## 6. Cómo ejecutar

```bash
cd FineAutomationInterviewDemo
dotnet run
```

Resultado esperado:
- 3 multas procesadas correctamente → `Output/Processed/`
- 1 multa fallida → cliente sin contrato

---

## 7. Cómo evolucionaría a producción

| Demo | Producción |
|---|---|
| `SampleFineFactory` | Selenium + parser PDF |
| `FakeContractRepository` | `SqlContractRepository` |
| `FakeServer/` | UNC path o blob storage |
| `ConsoleProcessingLogger` | `ILogger<T>` + Serilog |
| Sin emails | Servicio de notificación por idioma |

**La arquitectura no se reescribe**: se sustituyen implementaciones.

---

## Cómo defender esta demo en entrevista

> En el proyecto real, la descarga de multas se hacía con Selenium desde Pyramid y los datos venían de PDFs reales. Para esta demo he eliminado dependencias externas y datos privados. Creo multas manuales como objetos, indicando el origen de la multa. A partir de ahí, el sistema usa Strategy Pattern para aplicar reglas distintas según Málaga, Sevilla, Granada o DGT. Después consulta un repositorio simulado que representa SQL Server, valida las fechas del contrato, busca archivos en un servidor simulado y genera una carpeta final con la documentación. El orquestador no sabe de dónde vienen los datos ni cómo se guardan los archivos: solo coordina interfaces.

---

## Preguntas típicas de entrevista

### ¿Por qué usaste interfaces?
Para depender de abstracciones, no de implementaciones concretas. Facilita tests con mocks y sustituir fake por SQL sin tocar `FineProcessingService`.

### ¿Por qué usaste Strategy Pattern?
Cada origen tiene validaciones y formato de referencia distintos. Un `switch` en el orquestador violaría Open/Closed: cada nuevo origen obligaría a modificarlo.

### ¿Cómo cambiarías FakeContractRepository por SQL Server?

```csharp
public class SqlContractRepository : IContractRepository
{
    public Contract? GetByDni(string dni)
    {
        // SELECT ... WHERE Dni = @dni con ADO.NET / Dapper / EF Core
    }
}

// En ServiceCollectionExtensions:
services.AddSingleton<IContractRepository, SqlContractRepository>();
```

### ¿Cómo añadirías un nuevo origen?
1. Valor en `FineOrigin` (ej. `Cordoba`).
2. Clase `CordobaFineProcessor : IFineOriginProcessor`.
3. Registro: `services.AddSingleton<IFineOriginProcessor, CordobaFineProcessor>()`.

### ¿Cómo testearías este servicio?
- **Procesadores**: tests unitarios puros (sin mocks).
- **FineProcessingService**: mock de `IContractRepository`, `IServerFileService` con `IAppPaths` temporal.
- **ContractValidator**: tests con fechas dentro/fuera de rango.
- Assert sobre `ProcessingResult.Success` y mensajes.

### ¿Qué principio SOLID aplicas?

| Principio | Dónde |
|---|---|
| **S** | Cada clase una responsabilidad |
| **O** | Nuevos orígenes sin modificar orquestador |
| **L** | Cualquier `IFineOriginProcessor` es intercambiable |
| **I** | Interfaces pequeñas (`IContractValidator`, `IProcessingLogger`) |
| **D** | `FineProcessingService` depende de abstracciones |

### ¿Por qué no Selenium ni PDFs reales?
Porque la demo demuestra **diseño .NET**, no infraestructura. Selenium y PDFs distraen de DI, Strategy y Repository en una entrevista de 45 minutos.

### ¿Por qué `TryGetProcessor` y no excepción?
Los errores de negocio van por result objects. Un origen sin procesador es un fallo controlado que el orquestador convierte en `ProcessingResult.Fail`, no en un crash.

---

## Testing (opcional)

| Caso | Qué verificar |
|---|---|
| Multa con contrato existente | `Success = true`, carpeta creada |
| Multa sin contrato | Mensaje de DNI no encontrado |
| Multa fuera del rango de fechas | Error de periodo del contrato |
| Procesador Málaga | Referencia `MAL-{año}-{dni}` |
| Procesador DGT | Referencia `DGT-{yyyyMMdd}-{dni}` |

```bash
dotnet new xunit -n FineAutomationInterviewDemo.Tests
dotnet add FineAutomationInterviewDemo.Tests reference FineAutomationInterviewDemo
dotnet add FineAutomationInterviewDemo.Tests package Moq
dotnet test
```

---

## Referencias internas por origen

| Origen | Formato | Ejemplo |
|---|---|---|
| Málaga | `MAL-{año}-{dni}` | `MAL-2026-12345678A` |
| Sevilla | `SEV-{yyyyMMdd}-{dni}` | `SEV-20260603-Y7654321C` |
| Granada | `GRA-{yyyyMM}-{dni}` | `GRA-202606-00000000Z` |
| DGT | `DGT-{yyyyMMdd}-{dni}` | `DGT-20260602-X1234567B` |

---

## Licencia

Proyecto de demostración para fines educativos y entrevistas técnicas.
