using GestoreSpeseBudget.Data;
using GestoreSpeseBudget.Models;

namespace GestoreSpeseBudget.Repositories
{
    internal class SqliteExpenseRepository
    {
        private readonly ExpenseDbContext _context;

        public SqliteExpenseRepository()
        {
            _context = new ExpenseDbContext();
        }

        public void Add(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }

        public IReadOnlyList<Expense> GetAll()
        {
            return [.. _context.Expenses];
        }

        public bool Update(int id, decimal amount, string description)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null)
                return false;

            expense.Amount = amount;
            expense.Description = description;
            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null)
                return false;

            _context.Expenses.Remove(expense);
            _context.SaveChanges();

            return true;
        }
    }
}