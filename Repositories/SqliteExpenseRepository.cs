using GestoreSpeseBudget.Data;
using GestoreSpeseBudget.Models;
using Microsoft.EntityFrameworkCore;

namespace GestoreSpeseBudget.Repositories;

public class SqliteExpenseRepository(IDbContextFactory<ExpenseDbContext> contextFactory) : IExpenseRepository
{
    public IReadOnlyList<Expense> GetAll()
    {
        using var context = contextFactory.CreateDbContext();
        return [.. context.Expenses.AsNoTracking().OrderBy(e => e.Date)];
    }

    public IReadOnlyList<Expense> GetByCategory(Category category)
    {
        using var context = contextFactory.CreateDbContext();
        return [.. context.Expenses.AsNoTracking().Where(e => e.Category == category).OrderBy(e => e.Date)];
    }

    public Expense? GetById(int id)
    {
        using var context = contextFactory.CreateDbContext();
        return context.Expenses.AsNoTracking().FirstOrDefault(e => e.Id == id);
    }

    public decimal GetTotalAmount()
    {
        using var context = contextFactory.CreateDbContext();
        // Casts to nullable decimal to safeguard against null results when tables or filters are empty
        return context.Expenses.Sum(e => (decimal?)e.Amount) ?? 0m;
    }

    public decimal GetMonthlyTotal(int year, int month)
    {
        using var context = contextFactory.CreateDbContext();
        // Pushes aggregation to SQLite via SQL SUM, avoiding heap allocations of entire tables
        return context.Expenses
            .Where(e => e.Date.Year == year && e.Date.Month == month)
            .Sum(e => (decimal?)e.Amount) ?? 0m;
    }

    public Dictionary<Category, decimal> GetSummaryByCategory()
    {
        using var context = contextFactory.CreateDbContext();
        return context.Expenses
            .GroupBy(e => e.Category)
            .Select(group => new { Category = group.Key, Total = group.Sum(e => e.Amount) })
            .ToDictionary(item => item.Category, item => item.Total);
    }

    public void Add(Expense expense)
    {
        using var context = contextFactory.CreateDbContext();
        context.Expenses.Add(expense);
        context.SaveChanges();
    }

    public bool Update(int id, decimal amount, string description)
    {
        using var context = contextFactory.CreateDbContext();
        var existing = context.Expenses.Find(id);
        if (existing is null)
            return false;

        existing.UpdateDetails(amount, description);
        context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        using var context = contextFactory.CreateDbContext();
        var existing = context.Expenses.Find(id);
        if (existing is null)
            return false;

        context.Expenses.Remove(existing);
        context.SaveChanges();
        return true;
    }
}