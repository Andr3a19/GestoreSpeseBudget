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
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "L'importo della spesa deve essere maggiore di zero");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descrizione non può essere vuota", nameof(description));

        Amount = amount;
        Date = date;
        Description = description.Trim();
        Category = category;
    }

    protected Expense() { }

    public void UpdateDetails(decimal newAmount, string newDescription)
    {
        if (newAmount <= 0)
            throw new ArgumentOutOfRangeException(nameof(newAmount), "L'importo della spesa deve essere maggiore di zero");

        if (string.IsNullOrWhiteSpace(newDescription))
            throw new ArgumentException("La descrizione non può essere vuota", nameof(newDescription));

        Amount = newAmount;
        Description = newDescription.Trim();
    }

    public override string ToString() => $"[{Id}] {Date:dd/MM/yyyy} - {Description} | {Amount:C} ({Category})";
}