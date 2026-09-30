using GestoreSpeseBudget.Models;
using Microsoft.EntityFrameworkCore;

namespace GestoreSpeseBudget.Data
{
    internal class ExpenseDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }

        public ExpenseDbContext()
        {
            // Automatically create the local SQLite database file and tables on startup if they do not exist
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=expenses.db");
        }
    }
}