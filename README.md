# GestoreSpeseBudget - CLI Expense & Budget Manager

Un'applicazione console sviluppata in **C# 12** e **.NET** per la gestione e l'analisi del budget e delle spese personali.

Il progetto è stato sviluppato applicando i principi del **Clean Code** e dell'ingegneria del software, adottando l'architettura **MVC (Model-View-Controller)** e il **Repository Pattern** per disaccoppiare la logica applicativa dai meccanismi di persistenza dei dati.

---

## Funzionalità Principali

- **CRUD Completo:**
  - **Create:** inserimento di una nuova spesa con importo, data odierna automatica, descrizione e categoria.
  - **Read:** visualizzazione di tutte le spese registrate o filtrate per categoria.
  - **Update:** modifica mirata di importo e descrizione selezionando l'identificativo (ID).
  - **Delete:** cancellazione di una spesa per ID e sincronizzazione dell'avanzamento dei record.
- **Monitoraggio del Budget Mensile:** impostazione di un tetto di spesa mensile con calcolo dinamico del totale speso nel mese corrente, calcolo del saldo residuo e notifica immediata in caso di sforamento del budget.
- **Riepilogo e Aggregazione per Categoria:** calcolo dei totali aggregati per ciascuna categoria tramite query **LINQ (`GroupBy`)** mappati in un dizionario dedicato.
- **Esportazione in Formato CSV:** generazione automatica di un file `expenses.csv` formattato con delimitatore punto e virgola (`;`), direttamente compatibile con Microsoft Excel e software di foglio di calcolo.
- **Interfaccia Console Difensiva:** gestione robusta di tutti gli input da terminale (validazione di interi con limiti di intervallo, gestione dei decimali sia con virgola che con punto, prevenzione di crash da stringhe vuote o formati non validi).

---

## Architettura del Software

Il progetto segue rigorosamente la separazione delle responsabilità:

```text
GestoreSpeseBudget/
│
├── Models/
│   └── Expense.cs               # Entità di dominio Expense ed enum Category
│
├── Controllers/
│   └── ExpenseController.cs      # Coordinamento, logica applicativa e calcoli LINQ
│
├── Views/
│   └── ConsoleView.cs            # Interfaccia utente a terminale e formattazione dell'output
│
├── Repositories/
│   ├── SqliteExpenseRepository.cs # Persistenza relazionale su database SQLite con EF Core
│   ├── JsonExpenseRepository.cs   # Persistenza alternativa su file JSON tramite System.Text.Json
│   └── CsvExpenseExporter.cs      # Generatore di esportazione tabellare in formato CSV
│
├── Data/
│   └── ExpenseDbContext.cs       # Contesto del database Entity Framework Core
│
└── Program.cs                   # Entry point dell'applicazione (configurazione UTF-8 e Pure DI)
```

---

## Tecnologie e Pattern Adottati

- **Linguaggio e Framework:** C# 12 / .NET 8+
- **Database e ORM:** **Entity Framework Core (EF Core)** con provider **SQLite** (approccio Code-First con inizializzazione automatica tramite `Database.EnsureCreated()`).
- **Persistenza Alternativa:** serializzazione/deserializzazione nativa JSON con `System.Text.Json`.
- **Querying:** utilizzo intensivo di **LINQ** (`.Sum()`, `.Where()`, `.GroupBy()`, `.Max()`, `.Select()`).
- **Design Pattern e Principi di Progettazione:**
  - **MVC (Model-View-Controller):** disaccoppiamento totale tra presentazione e logica.
  - **Repository Pattern:** astrazione completa dell'accesso ai dati.
  - **Single Responsibility Principle (SRP):** ogni classe ha una sola responsabilità.
  - **Open/Closed Principle (OCP):** gestione dinamica delle categorie via reflection/enum (`Enum.GetValues<T>()`), consentendo l'estensione senza modifiche alla vista.
  - **DRY (Don't Repeat Yourself):** unificazione dei metodi di input (`EnterInt`, `EnterDecimal`) e di output (`Print`).
  - **Pure Dependency Injection:** iniezione del controller nel costruttore della vista a livello di `Program.cs`.
- **Funzionalità di C# Moderno:**
  - *Primary Constructors* su modelli e classi di vista
  - *Collection Expressions* (`[]`) e operatore *Spread* (`..`)
  - *Switch Expressions* per il mapping dei flussi
  - *Expression-Bodied Members* (`=>`) per metodi sintetici
  - Tipo `DateOnly` per la gestione temporale senza orario

---

## Come Avviare il Progetto in Locale

### Prerequisiti
- [.NET SDK 8.0](https://dotnet.microsoft.com/download) o superiore installato sul computer.

### Procedura

1. Clona il repository:
   ```bash
   git clone https://github.com/Andr3a19/GestoreSpeseBudget.git
   ```

2. Entra nella cartella di progetto:
   ```bash
   cd GestoreSpeseBudget
   ```

3. Ripristina le dipendenze NuGet:
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