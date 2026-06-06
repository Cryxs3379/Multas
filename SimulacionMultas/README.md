# SimulacionMultas

Simulación didáctica en **C# / .NET 8** para estudiar buenas prácticas sin Selenium, PDFs reales ni SQL Server.

No es una aplicación de producción. Es un **mapa del flujo real** reducido a lo esencial.

---

## 1. ¿Qué representa esta simulación?

En el proyecto real automatizabas multas de rent a car:

1. Selenium entraba en Pyramid y descargaba PDFs de una fecha.
2. Cada multa tenía un **origen** (Málaga, DGT, otro nuevo...).
3. Según el origen, se aplicaban **reglas distintas** (Strategy).
4. Se leía el PDF con **regex** y se creaba un objeto `Multa`.
5. Se consultaba **SQL Server** por DNI para obtener el `Contrato`.
6. Se creaba una **carpeta final** en el servidor con multa, contrato y ayudas.

Aquí todo es **fake**, pero la **estructura** es la que usarías en un proyecto profesional.

---

## 2. ¿Qué clase representa qué del proyecto real?

| Clase simulación | Proyecto real |
|---|---|
| `PyramidServiceFake` | Selenium + web Pyramid |
| `PdfExtractorServiceFake` | Lectura de PDF + texto para regex |
| `ProcesadorMultaMalaga` / `Dgt` | Reglas por ciudad/organismo |
| `ProcesadorMultaDefault` | Origen nuevo sin procesador específico |
| `ProcesadorMultaFactory` | Elegir procesador sin switch enorme |
| `ContratoRepositoryFake` | Consulta SQL Server por DNI |
| `FileStorageServiceFake` | Servidor de archivos / UNC |
| `ProcesadorMultas` | Orquestador del batch del día |
| `ConsoleLogger` | Logs del proceso |

---

## 3. Inyección de dependencias por constructor

`ProcesadorMultas` **no crea** sus colaboradores con `new`. Los **recibe** en el constructor:

```csharp
public ProcesadorMultas(
    IPyramidService pyramid,
    IPdfExtractorService pdfExtractor,
    ...
)
```

**¿Por qué?**

- Puedes cambiar `PyramidServiceFake` por `PyramidServiceReal` sin tocar el orquestador.
- Puedes testear con mocks.
- Las dependencias son **explícitas** (ves todo lo que necesita la clase).

En `Program.cs`, **Microsoft.Extensions.DependencyInjection** hace ese cableado por ti cuando registras:

```csharp
services.AddSingleton<IPyramidService, PyramidServiceFake>();
```

---

## 4. Dependency Inversion (explicado fácil)

**Sin DIP:** `ProcesadorMultas` usa directamente `new PyramidServiceFake()` → está acoplado a la implementación.

**Con DIP:** `ProcesadorMultas` depende de `IPyramidService` (abstracción) → no le importa si es fake o Selenium.

> *"Depende de abstracciones, no de detalles concretos."*

Eso es la **D** de SOLID.

---

## 5. Strategy Pattern (orígenes de multa)

Cada origen tiene su clase:

- `ProcesadorMultaMalaga` → referencia `MAL-...`
- `ProcesadorMultaDgt` → referencia `DGT-...`
- `ProcesadorMultaDefault` → referencia `GEN-...` para orígenes nuevos

`ProcesadorMultaFactory` elige cuál usar. El orquestador **no tiene** un `switch (origen)` gigante.

**Ventaja:** Añadir Sevilla = nueva clase + un registro en DI. No modificas `ProcesadorMultas`.

---

## 6. Repository Pattern (contratos)

`IContratoRepository` define `BuscarPorDni(string dni)`.

- Hoy: `ContratoRepositoryFake` (lista en memoria).
- Mañana: `ContratoRepositorySql` (SQL Server).

`ProcesadorMultas` solo llama a la interfaz. No sabe de SQL.

---

## 7. ¿Por qué ProcesadorMultas no debe hacerlo todo?

Si una clase descarga PDFs, aplica regex, consulta SQL y copia archivos:

- Es difícil de **entender**.
- Es difícil de **testear**.
- Un cambio en Pyramid **rompe** la lógica de contratos.

Separar responsabilidades:

| Clase | Una sola cosa |
|---|---|
| `ProcesadorMultas` | Orquestar el flujo |
| `IPyramidService` | Descargar |
| `IProcesadorMulta` | Reglas por origen |
| `IContratoRepository` | Datos de contrato |
| `IFileStorageService` | Archivos finales |

Eso es **Single Responsibility** (la **S** de SOLID).

---

## 8. Cómo defenderlo en una entrevista .NET

> "Automatizaba multas en un batch diario. Para estudiar la arquitectura hice una simulación: el orquestador `ProcesadorMultas` coordina el flujo pero delega en interfaces. Pyramid y PDFs son fake; los contratos vienen de un repository fake. Los orígenes usan Strategy con un procesador default para orígenes nuevos, como en producción. Todo se inyecta por constructor con DI. Si mañana cambio SQL o Selenium, solo cambio la implementación registrada en Program.cs."

---

## Estructura del proyecto

```
SimulacionMultas/
├── Models/           → Multa, Contrato, resultados
├── Interfaces/       → Contratos (abstracciones)
├── Services/         → Implementaciones fake + orquestador
├── Program.cs        → Registro DI y ejecución
└── README.md
```

---

## Cómo ejecutar

```bash
cd SimulacionMultas
dotnet run
```

Verás 4 multas simuladas: 3 éxitos y 1 error (DNI sin contrato).

---

## Qué NO incluye (a propósito)

- Clean Architecture con múltiples proyectos
- Base de datos real
- Selenium ni PDFs reales
- Tests (puedes añadirlos después como ejercicio)

**Prioridad: aprender con poco código y mucha claridad.**
