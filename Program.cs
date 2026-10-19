using GestoreSpeseBudget.Data;
using GestoreSpeseBudget.Repositories;
using GestoreSpeseBudget.Services;
using GestoreSpeseBudget.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace GestoreSpeseBudget;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var services = new ServiceCollection();

        services.AddDbContextFactory<ExpenseDbContext>(options =>
            options.UseSqlite("Data Source=expenses.db"));

        services.AddScoped<IExpenseRepository, SqliteExpenseRepository>();
        services.AddScoped<IExpenseExporter, CsvExpenseExporter>();
        services.AddScoped<ExpenseService>();
        services.AddScoped<ConsoleView>();

        var serviceProvider = services.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ExpenseDbContext>>();
            using var dbContext = factory.CreateDbContext();
            dbContext.Database.EnsureCreated();
        }

        var view = serviceProvider.GetRequiredService<ConsoleView>();
        view.Start();
    }
}