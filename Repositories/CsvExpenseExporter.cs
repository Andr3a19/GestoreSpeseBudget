using GestoreSpeseBudget.Models;

namespace GestoreSpeseBudget.Repositories
{
    internal class CsvExpenseExporter
    {
        private readonly string _filePath;

        public CsvExpenseExporter()
        {
            _filePath = "expenses.csv";
        }

        public void Export(IReadOnlyList<Expense> expenses)
        {
            string header = "Id;Data;Descrizione;Importo;Categoria";
            // Sanitize description by removing semicolons to avoid breaking CSV column alignment
            var rows = expenses.Select(e => $"{e.Id};{e.Date};{e.Description.Replace(';', ' ')};{e.Amount};{e.Category}");
            File.WriteAllLines(_filePath, [header, .. rows]);
        }
    }
}