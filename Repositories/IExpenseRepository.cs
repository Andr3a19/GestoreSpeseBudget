using GestoreSpeseBudget.Models;

namespace GestoreSpeseBudget.Repositories;

public interface IExpenseRepository
{
    IReadOnlyList<Expense> GetAll();
    IReadOnlyList<Expense> GetByCategory(Category category);
    decimal GetTotalAmount();
    decimal GetMonthlyTotal(int year, int month);
    IReadOnlyDictionary<Category, decimal> GetSummaryByCategory();
    void Add(Expense expense);
    bool Update(int id, decimal amount, string description);
    bool Delete(int id);
}