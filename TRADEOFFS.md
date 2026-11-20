# Trade-off Log - GestoreSpeseBudget

Questo documento registra le decisioni di design e i compromessi architetturali presi durante lo sviluppo del progetto.

| Decisione | Alternativa scartata | Perché | Data |
|---|---|---|---|
| Architettura a strati (View → Service → Repository) | Chiamata diretta al DbContext dalla ConsoleView | Disaccoppiamento per testabilità e coerenza con il pattern architetturale del portfolio, a costo di un layer in più (che agisce da Application Service) | 2026-10-21 |
| `IDbContextFactory` invece di `AddDbContext` Scoped | DbContext iniettato direttamente come Scoped | Evita memory leak e problemi di Change Tracker in un'app CLI long-running (il menu di console resta aperto all'infinito) | 2026-10-03 |
| `Math.Round` con `MidpointRounding.AwayFromZero` per gli importi | `ToEven` (Banker's rounding) | L'arrotondamento monetario commerciale standard (AwayFromZero) è più intuitivo per gli utenti finali e coerente con la convenzione bancaria italiana | 2026-10-21 |
| `IExpenseRepository` con una sola implementazione | Usare direttamente `ExpenseDbContext` nel Service | Il Repository Pattern permette di isolare le query SQL, centralizzare `AsNoTracking`, e permetterà di passare a un DB in-memory per i test futuri senza toccare il Service | 2026-10-21 |
| Upgrade a .NET 11 / C# 15 | Rimanere su .NET 10 / C# 14 | Allineamento all'ultima versione LTS. `with(capacity: ...)` non è applicabile a `IReadOnlyList<T>`, quindi la feature verrà sfruttata solo dove il target è mutabile (`List<T>`/`IList<T>`) | 2026-11-20 |