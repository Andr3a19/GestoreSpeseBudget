using GestoreSpeseBudget.Controllers;
using GestoreSpeseBudget.Views;

namespace GestoreSpeseBudget
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ExpenseController expenseController = new();
            ConsoleView view = new(expenseController);

            view.Start();
        }
    }
}