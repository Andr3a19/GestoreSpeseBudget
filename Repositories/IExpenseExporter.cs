using GestoreSpeseBudget.Models;

namespace GestoreSpeseBudget.Repositories;

public interface IExpenseExporter
{
    void Export(IReadOnlyList<Expense> expenses);
}