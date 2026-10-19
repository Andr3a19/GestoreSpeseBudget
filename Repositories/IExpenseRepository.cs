using GestoreSpeseBudget.Models;

namespace GestoreSpeseBudget.Repositories;

public interface IExpenseRepository
{
    IReadOnlyList<Expense> GetAll();
    IReadOnlyList<Expense> GetByCategory(Category category);
    Expense? GetById(int id);
    decimal GetTotalAmount();
    decimal GetMonthlyTotal(int year, int month);
    Dictionary<Category, decimal> GetSummaryByCategory();
    void Add(Expense expense);
    bool Update(int id, decimal amount, string description);
    bool Delete(int id);
}