namespace GestoreSpeseBudget.Models;

public class Expense
{
    public int Id { get; init; }

    public decimal Amount { get; private set; }
    public DateOnly Date { get; init; }
    public string Description { get; private set; } = string.Empty;
    public Category Category { get; init; }

    public Expense(decimal amount, DateOnly date, string description, Category category)
    {
        Amount = ValidateAmount(amount, nameof(amount));
        Date = date;
        Description = ValidateDescription(description, nameof(description));
        Category = category;
    }

    // Parameterless constructor required by EF Core for entity materialization
    protected Expense() { }

    public void UpdateDetails(decimal newAmount, string newDescription)
    {
        Amount = ValidateAmount(newAmount, nameof(newAmount));
        Description = ValidateDescription(newDescription, nameof(newDescription));
    }

    private static decimal ValidateAmount(decimal amount, string paramName)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(paramName, "L'importo della spesa deve essere maggiore di zero");

        return amount;
    }

    private static string ValidateDescription(string description, string paramName)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descrizione non può essere vuota", paramName);

        return description.Trim();
    }

    public override string ToString() => $"[{Id}] {Date:dd/MM/yyyy} - {Description} | {Amount:C} ({Category})";
}