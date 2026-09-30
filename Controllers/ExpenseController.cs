using GestoreSpeseBudget.Models;
using GestoreSpeseBudget.Repositories;

namespace GestoreSpeseBudget.Controllers
{
    internal class ExpenseController
    {
        private List<Expense> _expenseList;
        private readonly SqliteExpenseRepository _repository = new();
        private readonly CsvExpenseExporter _csvExporter = new();
        public decimal MonthlyBudget { get; set; }

        public ExpenseController()
        {
            _expenseList = [.. _repository.GetAll()];

            // Synchronize the static ID counter with the highest existing ID to prevent primary key collisions
            if (_expenseList.Count > 0)
                Expense.SetNextId(_expenseList.Max(e => e.Id) + 1);
        }

        public void AddExpense(decimal amount, DateOnly date, string description, Category category)
        {
            var expense = new Expense(amount, date, description, category);
            _repository.Add(expense);
            _expenseList.Add(expense);
        }

        public IReadOnlyList<Expense> GetExpenseList() => _expenseList;

        public decimal GetTotalAmount() => _expenseList.Sum(e => e.Amount);

        public IReadOnlyList<Expense> GetExpensesByCategory(Category category) => [.. _expenseList.Where(e => e.Category == category)];

        public bool DeleteExpense(int id)
        {
            if (_repository.Delete(id))
            {
                _expenseList = [.. _repository.GetAll()];
                Expense.SetNextId(_expenseList.Count > 0 ? _expenseList.Max(e => e.Id) + 1 : 1);
                return true;
            }

            return false;
        }

        public decimal GetMonthlyExpensesTotal()
        {
            return _expenseList
                .Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year)
                .Sum(e => e.Amount);
        }

        public decimal GetRemainingBudget() => MonthlyBudget - GetMonthlyExpensesTotal();

        public Dictionary<Category, decimal> GetExpensesSummaryByCategory() => _expenseList
                .GroupBy(e => e.Category)
                .ToDictionary(group => group.Key, group => group.Sum(e => e.Amount));

        public bool UpdateExpense(int id, decimal amount, string description)
        {
            if (_repository.Update(id, amount, description))
            {
                _expenseList = [.. _repository.GetAll()];
                return true;
            }

            return false;
        }

        public void ExportToCsv() => _csvExporter.Export(_expenseList);
    }
}