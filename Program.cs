using GestoreSpeseBudget.Data;
using GestoreSpeseBudget.Exporters;
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

        services.AddSingleton<IExpenseRepository, SqliteExpenseRepository>();
        services.AddSingleton<CsvExpenseExporter>();
        services.AddSingleton<ExpenseService>();
        services.AddSingleton<ConsoleView>();

        using var serviceProvider = services.BuildServiceProvider();

        var factory = serviceProvider.GetRequiredService<IDbContextFactory<ExpenseDbContext>>();
        using (var dbContext = factory.CreateDbContext())
        {
            dbContext.Database.EnsureCreated();
        }

        serviceProvider.GetRequiredService<ConsoleView>().Start();
    }
}