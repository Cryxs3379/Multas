# FineAutomationInterviewDemo

Demo técnica en **C# / .NET 8** para practicar y demostrar buenas prácticas en una entrevista técnica .NET.

Basada en un caso real de automatización de multas para una empresa de rent a car, pero **simplificada y anonimizada** para poder explicarla sin dependencias externas ni datos privados.

---

## 1. ¿Qué problema resuelve?

Automatiza el procesamiento posterior a la descarga de una multa:

1. Aplica reglas de negocio según el **origen** de la multa (Málaga, Sevilla, Granada o DGT).
2. Busca el **contrato** del cliente por DNI.
3. Valida que la fecha de la multa esté dentro del periodo del contrato.
4. Recupera archivos del **servidor simulado** (multa, contrato, documento de ayuda por idioma).
5. Genera una **carpeta final** con toda la documentación lista para gestión.

En el proyecto real, antes de este flujo había Selenium, lectura de PDFs y consultas a SQL Server. Aquí la demo **empieza cuando la multa ya está parseada**, representada como un objeto `Fine`.

---

## 2. ¿Por qué es una versión simplificada?

| Sistema real | Demo |
|---|---|
| Selenium + web Pyramid | Multas creadas manualmente como objetos |
| PDFs con regex | Archivos `.txt` simulados |
| SQL Server | `FakeContractRepository` en memoria |
| Servidor de archivos corporativo | Carpeta local `FakeServer/` |
| Datos reales de clientes | Datos ficticios y anonimizados |

La simplificación permite centrarse en **arquitectura, patrones y buenas prácticas** sin infraestructura externa.

---

## 3. Partes simuladas

- **Descarga de multas** → archivos en `FakeServer/DownloadedFines/`
- **Base de datos de contratos** → `FakeContractRepository`
- **Servidor de archivos** → `FakeServer/Contracts/` y `FakeServer/HelpDocuments/`
- **Reglas por origen** → procesadores en `Strategies/`

---

## 4. Arquitectura del proyecto

```
FineAutomationInterviewDemo/
 ├── Program.cs                    # Composición raíz (DI + ejecución)
 ├── Models/                       # Entidades de dominio
 ├── Enums/                        # FineOrigin, Language
 ├── Repositories/                 # Repository Pattern (simulado)
 ├── Services/                     # Orquestación y archivos
 ├── Strategies/                   # Strategy Pattern por origen
 ├── Infrastructure/               # Seeder del sistema de archivos fake
 ├── FakeServer/                   # Datos simulados (generados al ejecutar)
 └── Output/Processed/             # Resultado del procesamiento
```

### Responsabilidades

| Capa | Responsabilidad |
|---|---|
| `Program.cs` | Configurar DI, sembrar datos, ejecutar demo |
| `Strategies/` | Reglas específicas por origen de multa |
| `Repositories/` | Acceso a contratos (intercambiable por SQL) |
| `Services/` | Orquestación, rutas de servidor y carpeta final |
| `Infrastructure/` | Crear estructura fake si no existe |

---

## 5. Flujo de ejecución

```
Fine (manual)
    │
    ▼
FineProcessingService
    │
    ├─► FineOriginProcessorFactory → IFineOriginProcessor (Strategy)
    │       └─► OriginProcessingResult (referencia interna)
    │
    ├─► IContractRepository.GetByDni()
    │
    ├─► Validación de fechas del contrato
    │
    ├─► IServerFileService (multa, contrato, ayuda)
    │
    └─► IOutputFolderService (carpeta + copias + resumen.txt)
            │
            ▼
    ProcessingResult
```

---

## 6. Buenas prácticas demostradas

- **Clases y modelos** bien definidos (`Fine`, `Contract`, `ProcessingResult`)
- **Enums** para orígenes e idiomas
- **LINQ** en el repositorio (`FirstOrDefault`)
- **Interfaces** para desacoplar implementaciones
- **Servicios** con responsabilidad única
- **Inyección de dependencias** con `Microsoft.Extensions.DependencyInjection`
- **Separación de responsabilidades** (sin lógica de negocio en `Program.cs`)
- **Principios SOLID** (ver sección de entrevista)
- **Validaciones de negocio** en procesadores y servicio
- **Manejo controlado de errores** con `ProcessingResult` / `OriginProcessingResult`
- **Gestión de archivos** encapsulada en servicios dedicados
- **Código preparado para evolucionar** a SQL Server, Selenium y PDFs reales

---

## 7. Patrones utilizados

### Strategy Pattern
Cada origen (`Malaga`, `Sevilla`, `Granada`, `DGT`) tiene su propio procesador que implementa `IFineOriginProcessor`. El orquestador no usa `switch` enormes: delega en `FineOriginProcessorFactory`.

### Repository Pattern
`IContractRepository` abstrae el acceso a contratos. Hoy es `FakeContractRepository`; mañana puede ser `SqlContractRepository` sin tocar el servicio principal.

### Dependency Injection
Todas las dependencias se registran en `Program.cs` y se inyectan por constructor. Facilita testing y sustitución de implementaciones.

### Separación de responsabilidades
- `FineProcessingService` → orquesta el flujo
- `IFineOriginProcessor` → reglas por origen
- `IServerFileService` → rutas y existencia de archivos
- `IOutputFolderService` → carpeta final y resumen

---

## 8. Cómo ejecutar

```bash
cd FineAutomationInterviewDemo
dotnet run
```

La primera ejecución crea automáticamente los archivos en `FakeServer/` mediante `FakeFileSystemSeeder`.

Se procesan 4 multas de ejemplo:
- 3 casos exitosos (Juan, John, Marie)
- 1 caso de error (cliente sin contrato)

Las carpetas resultantes aparecen en `Output/Processed/`.

---

## 9. Cómo evolucionaría a una versión real

| Componente demo | Evolución real |
|---|---|
| Objetos `Fine` manuales | Selenium en Pyramid + descarga de PDFs |
| Archivos `.txt` | Parser de PDF con regex o librería especializada |
| `FakeContractRepository` | `SqlContractRepository` con ADO.NET / Dapper / EF Core |
| `FakeServer/` local | UNC path o API de almacenamiento corporativo |
| `Console.WriteLine` | `ILogger<T>` con Serilog / Application Insights |
| Sin notificaciones | Servicio de email al cliente según idioma |

La estructura actual **no habría que reescribirla**: solo sustituir implementaciones concretas manteniendo las interfaces.

---

## 10. Cómo explicarlo en una entrevista técnica

1. **Contexto**: "Automatizaba multas de rent a car. El flujo real empezaba en Pyramid con Selenium."
2. **Simplificación**: "Para la demo eliminé dependencias externas y empiezo cuando la multa ya está parseada."
3. **Strategy**: "Cada ciudad/DGT tiene reglas distintas. Uso Strategy para no acoplar el orquestador."
4. **Repository**: "El contrato viene de una abstracción. Hoy es fake, en producción sería SQL Server."
5. **Flujo**: "Valido origen → busco contrato → valido fechas → recupero archivos → genero carpeta final."
6. **SOLID**: "Open/Closed al añadir orígenes, Dependency Inversion con interfaces, Single Responsibility por clase."
7. **Evolución**: "Cambiaría implementaciones, no la arquitectura."

---

## Cómo defender esta demo en entrevista

> En el proyecto real, la descarga de multas se hacía con Selenium desde Pyramid y los datos venían de PDFs reales. Para esta demo he eliminado dependencias externas y datos privados. Creo multas manuales como objetos, indicando el origen de la multa. A partir de ahí, el sistema usa Strategy Pattern para aplicar reglas distintas según Málaga, Sevilla, Granada o DGT. Después consulta un repositorio simulado que representa SQL Server, valida las fechas del contrato, busca archivos en un servidor simulado y genera una carpeta final con la documentación.

---

## Preguntas típicas de entrevista que puedo responder con este proyecto

### ¿Por qué usaste interfaces?
Para desacoplar la lógica de negocio de las implementaciones concretas. `IFineProcessingService` no sabe si el repositorio es fake o SQL. Eso facilita tests, mantenimiento y sustitución sin romper el orquestador.

### ¿Por qué usaste Strategy Pattern?
Porque cada origen de multa tiene reglas diferentes (validaciones y formato de referencia interna). En lugar de un `switch` gigante en el servicio, cada origen tiene su clase. Añadir Granada, Sevilla o un quinto origen no modifica los existentes (Open/Closed).

### ¿Cómo cambiarías FakeContractRepository por SQL Server?
Crearía `SqlContractRepository : IContractRepository` con ADO.NET, Dapper o EF Core. En `Program.cs` cambiaría el registro:

```csharp
services.AddSingleton<IContractRepository, SqlContractRepository>();
```

`FineProcessingService` no cambiaría.

### ¿Cómo añadirías un nuevo origen de multa?
1. Añadir valor al enum `FineOrigin` (ej. `Cordoba`).
2. Crear `CordobaFineProcessor : IFineOriginProcessor`.
3. Registrar en DI: `services.AddSingleton<IFineOriginProcessor, CordobaFineProcessor>()`.

La factory lo detecta automáticamente vía `IEnumerable<IFineOriginProcessor>`.

### ¿Cómo testearías este servicio?
Con **xUnit** o **NUnit** y mocks (Moq / NSubstitute):

- Mock de `IContractRepository` para simular contrato encontrado / no encontrado.
- Procesadores reales o aislados para validar referencias (`MAL-2026-12345678A`).
- `ServerFileService` con rutas temporales (`Path.GetTempPath()`).
- Assert sobre `ProcessingResult.Success`, mensajes y rutas generadas.

### ¿Qué principio SOLID estás aplicando?

| Principio | Ejemplo en el proyecto |
|---|---|
| **S** – Single Responsibility | Cada procesador solo aplica reglas de su origen |
| **O** – Open/Closed | Nuevos orígenes sin modificar `FineProcessingService` |
| **L** – Liskov Substitution | Cualquier `IFineOriginProcessor` es intercambiable |
| **I** – Interface Segregation | Interfaces pequeñas y focalizadas |
| **D** – Dependency Inversion | El servicio depende de abstracciones, no de fakes |

### ¿Por qué no has usado Selenium ni PDFs reales en la demo?
Porque el objetivo es demostrar **arquitectura y patrones .NET**, no infraestructura externa. Selenium y PDFs añaden complejidad operativa (drivers, credenciales, datos sensibles) que distrae de lo que quiero explicar: DI, Strategy, Repository y orquestación limpia.

---

## Testing (opcional)

No se incluye proyecto de tests para mantener la demo pequeña, pero estos serían los casos prioritarios:

| Caso | Qué verificar |
|---|---|
| Multa con contrato existente | `Success = true`, carpeta creada |
| Multa sin contrato | Mensaje de DNI no encontrado |
| Multa fuera del rango de fechas | Error de periodo del contrato |
| Procesador Málaga | Referencia `MAL-{año}-{dni}` |
| Procesador DGT | Referencia `DGT-{yyyyMMdd}-{dni}` |

### Ejemplo de estructura de tests

```
FineAutomationInterviewDemo.Tests/
 ├── MalagaFineProcessorTests.cs
 ├── DgtFineProcessorTests.cs
 └── FineProcessingServiceTests.cs
```

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
