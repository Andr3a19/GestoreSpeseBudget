using GestoreSpeseBudget.Models;
using GestoreSpeseBudget.Services;
using System.Globalization;

namespace GestoreSpeseBudget.Views;

public class ConsoleView(ExpenseService service)
{
    public void Start()
    {
        bool isRunning = true;
        SetMonthlyBudget();

        while (isRunning)
        {
            Console.WriteLine("\n1. Mostra tutte le spese");
            Console.WriteLine("2. Aggiungi nuova spesa");
            Console.WriteLine("3. Visualizza totale speso");
            Console.WriteLine("4. Filtra spese per categoria");
            Console.WriteLine("5. Elimina una spesa");
            Console.WriteLine("6. Stato del budget");
            Console.WriteLine("7. Riepilogo spese per categoria");
            Console.WriteLine("8. Modifica una spesa");
            Console.WriteLine("9. Esporta in formato CSV");
            Console.WriteLine("0. Esci");
            Console.Write("\nSeleziona un'opzione valida: ");

            var choice = EnterInt(0, 9);

            switch (choice)
            {
                case 0:
                    isRunning = false;
                    break;
                case 1:
                    ShowAllExpenses();
                    break;
                case 2:
                    AddNewExpense();
                    break;
                case 3:
                    ShowTotalAmount();
                    break;
                case 4:
                    ShowExpensesByCategory();
                    break;
                case 5:
                    DeleteExpense();
                    break;
                case 6:
                    ShowBudgetStatus();
                    break;
                case 7:
                    ShowExpensesSummaryByCategory();
                    break;
                case 8:
                    UpdateExpense();
                    break;
                case 9:
                    ExportToCsv();
                    break;
            }
        }
    }

    private void SetMonthlyBudget()
    {
        Console.Write("\nInserire il budget mensile: ");
        service.MonthlyBudget = EnterDecimal();
    }

    private void ShowAllExpenses()
    {
        var expenses = service.GetExpenses();
        Print(expenses);
    }

    private void AddNewExpense()
    {
        Console.Write("\nInserire il valore dell'importo: ");
        var amount = EnterDecimal();

        var description = EnterString("\nInserire la descrizione della spesa: ");
        var category = ChooseCategory();

        service.AddExpense(amount, DateOnly.FromDateTime(DateTime.Today), description, category);
        Console.WriteLine("\nSpesa aggiunta con successo");
    }

    private void ShowTotalAmount()
    {
        Console.WriteLine($"\nIl totale delle spese è {service.GetTotalAmount():C}");
    }

    private void ShowExpensesByCategory()
    {
        var category = ChooseCategory();
        var expenses = service.GetExpensesByCategory(category);
        Print(expenses);
    }

    private void DeleteExpense()
    {
        var expenses = service.GetExpenses();
        if (expenses.Count == 0)
        {
            Console.WriteLine("\nNessuna spesa presente in archivio: operazione non disponibile");
            return;
        }

        Print(expenses);
        Console.Write("\nIndicare quale spesa vuoi eliminare: ");
        var id = EnterInt(1);

        if (service.DeleteExpense(id))
            Console.WriteLine("\nSpesa eliminata con successo");
        else
            Console.WriteLine($"\nNessuna spesa trovata con ID {id}");
    }

    private void ShowBudgetStatus()
    {
        var totalMonthExpenses = service.GetMonthlyExpensesTotal();
        var remaining = service.CalculateRemainingBudget(totalMonthExpenses);

        Console.WriteLine($"\nLa somma delle spese di questo mese è di {totalMonthExpenses:C}");

        if (remaining >= 0)
        {
            Console.WriteLine($"Il budget mensile rimasto è di {remaining:C}");
            return;
        }

        Console.WriteLine($"Attenzione: superato il budget mensile, sei sotto di {Math.Abs(remaining):C}");
    }

    private void ShowExpensesSummaryByCategory()
    {
        var summary = service.GetExpensesSummaryByCategory();
        if (summary.Count == 0)
        {
            Console.WriteLine("\nNessuna spesa presente in archivio");
            return;
        }

        Console.WriteLine("\nRiepilogo spese:");
        foreach (var (category, total) in summary)
        {
            Console.WriteLine($"{category,-14}: {total:C}");
        }
    }

    private void UpdateExpense()
    {
        var expenses = service.GetExpenses();
        if (expenses.Count == 0)
        {
            Console.WriteLine("\nNessuna spesa presente in archivio: operazione non disponibile");
            return;
        }

        Print(expenses);
        Console.Write("\nIndicare quale spesa vuoi modificare: ");
        var id = EnterInt(1);

        // Ensures the expense exists before prompting for updated values
        if (!expenses.Any(e => e.Id == id))
        {
            Console.WriteLine($"\nNessuna spesa trovata con ID {id}");
            return;
        }

        Console.Write("\nInserire il nuovo valore dell'importo: ");
        var amount = EnterDecimal();

        var description = EnterString("\nInserire la nuova descrizione della spesa: ");

        service.UpdateExpense(id, amount, description);
        Console.WriteLine("\nSpesa modificata con successo");
    }

    private void ExportToCsv()
    {
        if (service.ExportToCsv())
            Console.WriteLine("\nFile 'expenses.csv' esportato con successo");
        else
            Console.WriteLine("\nNessuna spesa presente in archivio: operazione non disponibile");
    }

    private static void Print(IReadOnlyList<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("\nNessuna spesa trovata");
            return;
        }

        Console.WriteLine("\nElenco spese:");
        foreach (var expense in expenses)
            Console.WriteLine(expense);
    }

    private static Category ChooseCategory()
    {
        var categories = Enum.GetValues<Category>();

        Console.WriteLine("\nScegliere la categoria della spesa:");
        for (int i = 0; i < categories.Length; i++)
            Console.WriteLine($"{i + 1}. {categories[i]}");

        Console.Write("\nSeleziona un'opzione valida: ");
        var choice = EnterInt(1, categories.Length);

        return categories[choice - 1];
    }

    private static string EnterString(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim() ?? string.Empty;

        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Il testo non può essere vuoto");
            Console.Write("Nuovo valore: ");
            input = Console.ReadLine()?.Trim() ?? string.Empty;
        }

        return input;
    }

    private static decimal EnterDecimal()
    {
        while (true)
        {
            var rawInput = Console.ReadLine()?.Trim() ?? string.Empty;
            var normalizedInput = rawInput.Replace(',', '.');

            if (decimal.TryParse(normalizedInput, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsed))
            {
                // Rounds to two decimals to represent currency cents accurately
                var amount = Math.Round(parsed, 2, MidpointRounding.AwayFromZero);
                if (amount > 0)
                    return amount;
            }

            Console.WriteLine("Valore inserito non valido");
            Console.Write("Nuovo valore: ");
        }
    }

    private static int EnterInt(int min, int max = int.MaxValue)
    {
        int value;
        while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
        {
            Console.WriteLine("Valore inserito non valido");
            Console.Write("Nuovo valore: ");
        }

        return value;
    }
}