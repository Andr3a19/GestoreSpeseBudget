# GestoreSpeseBudget - CLI Expense & Budget Manager

Un'applicazione console sviluppata in **C# 14** e **.NET 10 (LTS)** per la gestione, l'analisi e il monitoraggio del budget e delle spese personali.

Il progetto adotta un'**Architettura a Strati (Layered Architecture)**, principi del **Domain-Driven Design (DDD)** e il **Repository Pattern**, disaccoppiando nettamente la logica di business, l'interfaccia utente a riga di comando e la persistenza dei dati.

---

## Architettura del Software

La soluzione è strutturata seguendo rigorosamente la separazione delle responsabilità (SRP) e l'inversione delle dipendenze (DIP):

```text
GestoreSpeseBudget/
│
├── Models/
│   ├── Category.cs               # Enum delle categorie di spesa
│   └── Expense.cs                # Entità di dominio incapsulata (Rich Domain Model)
│
├── Services/
│   └── ExpenseService.cs         # Servizio applicativo e logica di coordinamento budget
│
├── Repositories/
│   ├── IExpenseRepository.cs     # Contratto per l'accesso ai dati e aggregazioni
│   ├── SqliteExpenseRepository.cs# Implementazione relazionale su SQLite tramite EF Core
│   ├── IExpenseExporter.cs       # Contratto per l'esportazione dati
│   └── CsvExpenseExporter.cs     # Esportatore conforme allo standard RFC 4180
│
├── Data/
│   └── ExpenseDbContext.cs       # Contesto Entity Framework Core con configurazioni fluent
│
├── Views/
│   └── ConsoleView.cs            # Livello di presentazione a terminale e validazione input difensiva
│
└── Program.cs                    # Composition Root, Dependency Injection e bootstrap database
```

---

## Caratteristiche e Soluzioni Ingegneristiche

### 1. Dominio Incapsulato (Domain Invariants Protection)
L'entità `Expense` non adotta il pattern anemico: espone proprietà a mutazione controllata (`private set` / `init`) e metodi di mutazione espliciti (`UpdateDetails`). Lo stato interno dell'oggetto non può mai essere corrotto (importi negativi o descrizioni vuote vengono respinti all'origine).

### 2. Gestione Robusta dell'ORM (EF Core & SQLite)
- **Ciclo di Vita Sicuro tramite `IDbContextFactory`:** Anziché mantenere istanze a lunga durata, il repository crea istanze effimere (*Unit of Work* usa-e-getta) per azzerare rischi di memory leak e disallineamenti del Change Tracker.
- **SQL Aggregation Pushdown:** Calcoli come il totale complessivo, i totali mensili e i raggruppamenti per categoria non scaricano intere tabelle in memoria (evitando l'anti-pattern *In-Memory Dumping*), ma delegano il calcolo a SQLite tramite query native `SUM` e `GROUP BY` ($O(1)$ di overhead in memoria).
- **AsNoTracking:** Applicato su tutte le query in sola lettura per ottimizzare le prestazioni di estrazione.

### 3. Inversion of Control & Dependency Injection
Tutte le dipendenze sono gestite tramite il container ufficiale `Microsoft.Extensions.DependencyInjection`, consentendo l'estensibilità e la testabilità modulare del software (mocking dei repository e degli exporter).

### 4. Esportazione CSV Conforme a RFC 4180
L'esportazione in `expenses.csv` adotta la codifica UTF-8 e include un meccanismo deterministico di escaping delle virgolette doppie e dei delimitatori (`;`), garantendo piena compatibilità con Microsoft Excel e altri fogli di calcolo.

### 5. Validazione Difensiva dell'Input Utente
L'interfaccia a terminale normalizza i separatori decimali (consentendo sia il punto che la virgola indipendentemente dalla `CultureInfo` dell'OS) e valida costantemente gli intervalli di scelta e la non-vacuità delle stringhe per prevenire crash non gestiti a runtime.

---

## Tecnologie Utilizzate

- **Linguaggio:** C# 14
- **Runtime:** .NET 10.0 SDK (LTS)
- **Data Access:** Entity Framework Core 10+ (Provider SQLite)
- **Dependency Injection:** Microsoft.Extensions.DependencyInjection
- **Librerie di Supporto:** Microsoft.EntityFrameworkCore.Sqlite

---

## Come Avviare il Progetto in Locale

### Prerequisiti
- [.NET SDK 10.0](https://dotnet.microsoft.com/download) installato sul sistema.

### Installazione ed Esecuzione

1. Clona il repository:
   ```bash
   git clone https://github.com/Andr3a19/GestoreSpeseBudget.git
   ```

2. Accedi alla cartella del progetto:
   ```bash
   cd GestoreSpeseBudget
   ```

3. Ripristina i pacchetti NuGet:
   ```bash
   dotnet restore
   ```

4. Compila il progetto:
   ```bash
   dotnet build
   ```

5. Avvia l'applicazione:
   ```bash
   dotnet run
   ```