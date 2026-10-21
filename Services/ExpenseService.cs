using GestoreSpeseBudget.Exporters;
using GestoreSpeseBudget.Models;
using GestoreSpeseBudget.Repositories;

namespace GestoreSpeseBudget.Services;

public class ExpenseService(IExpenseRepository repository, CsvExpenseExporter csvExporter)
{
    public decimal MonthlyBudget { get; set; }

    public void AddExpense(decimal amount, DateOnly date, string description, Category category)
    {
        var expense = new Expense(amount, date, description, category);
        repository.Add(expense);
    }

    public IReadOnlyList<Expense> GetExpenses() => repository.GetAll();

    public decimal GetTotalAmount() => repository.GetTotalAmount();

    public IReadOnlyList<Expense> GetExpensesByCategory(Category category) =>
        repository.GetByCategory(category);

    public bool DeleteExpense(int id) => repository.Delete(id);

    public decimal GetMonthlyExpensesTotal()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return repository.GetMonthlyTotal(today.Year, today.Month);
    }

    public decimal CalculateRemainingBudget(decimal monthlyTotal) => MonthlyBudget - monthlyTotal;

    public IReadOnlyDictionary<Category, decimal> GetExpensesSummaryByCategory() =>
        repository.GetSummaryByCategory();

    public bool UpdateExpense(int id, decimal amount, string description) =>
        repository.Update(id, amount, description);

    public bool ExportToCsv()
    {
        var expenses = repository.GetAll();
        if (expenses.Count == 0)
            return false;

        csvExporter.Export(expenses);
        return true;
    }
}