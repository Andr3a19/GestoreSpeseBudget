using GestoreSpeseBudget.Controllers;
using GestoreSpeseBudget.Models;

namespace GestoreSpeseBudget.Views
{
    internal class ConsoleView(ExpenseController controller)
    {
        public void Start()
        {
            bool isRunning = true;
            int choice;
            SetMonthlyBudget();

            while (isRunning)
            {
                Console.WriteLine("\n1. Mostra tutte le spese");
                Console.WriteLine("2. Aggiungi nuova spesa");
                Console.WriteLine("3. Visualizza totale speso");
                Console.WriteLine("4. Filtra spese per categoria");
                Console.WriteLine("5. Elimina una spesa");
                Console.WriteLine("6. Budget status");
                Console.WriteLine("7. Riepilogo spese per categoria");
                Console.WriteLine("8. Modifica una spesa");
                Console.WriteLine("9. Esporta in formato CSV");
                Console.WriteLine("0. Esci");
                Console.Write("\nSeleziona un'opzione valida: ");
                choice = EnterInt(0, 9);

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
                        ShowForCategory();
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
            controller.MonthlyBudget = EnterDecimal();
        }

        private void ShowAllExpenses()
        {
            var expenses = controller.GetExpenseList();
            Print(expenses);
        }

        private void AddNewExpense()
        {
            Console.Write("\nInserire il valore dell'importo: ");
            decimal amount = EnterDecimal();

            string description;
            Console.Write("\nInserire la descrizione della spesa: ");
            description = Console.ReadLine() ?? "";

            controller.AddExpense(amount, DateOnly.FromDateTime(DateTime.Now), description, ChooseCategory());
            Console.WriteLine("Spesa aggiunta con successo\n");
        }

        private void ShowTotalAmount()
        {
            Console.WriteLine($"Il totale delle spese è {controller.GetTotalAmount():C}");
        }

        private void ShowForCategory()
        {
            var expenses = controller.GetExpensesByCategory(ChooseCategory());
            Print(expenses);
        }

        private void DeleteExpense()
        {
            var expenses = controller.GetExpenseList();

            if (expenses.Count == 0)
            {
                Console.WriteLine("\nNessuna spesa presente in archivio: operazione non disponibile");
                return;
            }

            Print(expenses);
            Console.Write("\nIndicare quale spesa vuoi eliminare: ");

            int choice = EnterInt(1);

            if (controller.DeleteExpense(choice))
                Console.WriteLine("Spesa eliminata con successo");
            else
                Console.WriteLine($"Nessuna spesa trovata con ID {choice}");
        }

        private void ShowBudgetStatus()
        {
            decimal totalMonthExpenses = controller.GetMonthlyExpensesTotal();
            decimal amount = controller.GetRemainingBudget();

            Console.WriteLine($"\nLa somma delle spese di questo mese è di {totalMonthExpenses:C}");

            if (amount >= 0)
            {
                Console.WriteLine($"Il budget mensile rimasto è di {amount:C}");
                return;
            }

            Console.WriteLine($"Attenzione superato il budget mensile, sei sotto di {Math.Abs(amount):C}");
        }

        private void ShowExpensesSummaryByCategory()
        {
            var dictionary = controller.GetExpensesSummaryByCategory();

            if (dictionary.Count == 0)
            {
                Console.WriteLine("\nNessuna spesa presente in archivio");
                return;
            }

            foreach (var item in dictionary)
                Console.WriteLine($"{item.Key,-12}: {item.Value:C}");
        }

        private void UpdateExpense()
        {
            var expenses = controller.GetExpenseList();

            if (expenses.Count == 0)
            {
                Console.WriteLine("\nNessuna spesa presente in archivio: operazione non disponibile");
                return;
            }

            Print(expenses);
            Console.Write("\nIndicare quale spesa vuoi modificare: ");

            int choice = EnterInt(1);

            Console.Write("\nInserire il nuovo valore dell'importo: ");
            decimal amount = EnterDecimal();
            string description;
            Console.Write("\nInserire la nuova descrizione della spesa: ");
            description = Console.ReadLine() ?? "";

            if (controller.UpdateExpense(choice, amount, description))
                Console.WriteLine("Spesa modificata con successo");
            else
                Console.WriteLine($"Nessuna spesa trovata con ID {choice}");
        }

        private void ExportToCsv()
        {
            var expenses = controller.GetExpenseList();

            if (expenses.Count == 0)
            {
                Console.WriteLine("\nNessuna spesa presente in archivio: operazione non disponibile");
                return;
            }

            controller.ExportToCsv();
            Console.WriteLine("\nFile 'expenses.csv' esportato con successo");
        }

        private static void Print(IReadOnlyList<Expense> expenses)
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("\nNessuna spesa trovata");
                return;
            }

            Console.WriteLine("Elenco spese:");
            foreach (var expense in expenses)
                Console.WriteLine(expense);
            Console.WriteLine("\n");
        }

        private static Category ChooseCategory()
        {
            var categories = Enum.GetValues<Category>();

            Console.WriteLine("Scegliere la categoria della spesa:");
            for (int i = 0; i < categories.Length; i++)
                Console.WriteLine($"{i + 1}. {categories[i]}");

            Console.Write("\nSeleziona un'opzione valida: ");
            int choice = EnterInt(1, categories.Length);

            return categories[choice - 1];
        }

        private static decimal EnterDecimal()
        {
            decimal amount;
            // Handle numpad input by converting dots to commas for reliable decimal parsing
            string input = Console.ReadLine()?.Replace('.', ',') ?? "";
            while (!decimal.TryParse(input, out amount) || amount <= 0)
            {
                Console.WriteLine("Valore inserito non è valido");
                Console.Write("Nuovo valore: ");
                input = Console.ReadLine()?.Replace('.', ',') ?? "";
            }

            return amount;
        }

        private static int EnterInt(int min, int max = int.MaxValue)
        {
            int value;
            while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
            {
                Console.WriteLine("Valore inserito non è valido");
                Console.Write("Nuovo valore: ");
            }
            return value;
        }
    }
}