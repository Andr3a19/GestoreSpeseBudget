using GestoreSpeseBudget.Models;
using System.Globalization;
using System.Text;

namespace GestoreSpeseBudget.Repositories;

public class CsvExpenseExporter(string filePath = "expenses.csv") : IExpenseExporter
{
    public void Export(IReadOnlyList<Expense> expenses)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Id;Data;Descrizione;Importo;Categoria");

        foreach (var e in expenses)
        {
            var line = string.Join(";",
                e.Id,
                e.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                EscapeCsvField(e.Description),
                e.Amount.ToString("F2", CultureInfo.InvariantCulture),
                e.Category.ToString()
            );
            builder.AppendLine(line);
        }

        File.WriteAllText(filePath, builder.ToString(), Encoding.UTF8);
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(';') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
            return $"\"{field.Replace("\"", "\"\"")}\"";

        return field;
    }
}