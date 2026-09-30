namespace GestoreSpeseBudget.Models
{
    public enum Category
    {
        Cibo,
        Trasporti,
        Svago,
        Bollette,
        Altro
    }
    public class Expense(decimal amount, DateOnly date, string description, Category category)
    {
        private static int _nextId = 1;
        public int Id { get; set; } = _nextId++;
        public decimal Amount { get; set; } = amount;
        public DateOnly Date { get; set; } = date;
        public string Description { get; set; } = description;
        public Category Category { get; set; } = category;

        public static void SetNextId(int id)
        {
            _nextId = id;
        }

        public override string ToString()
        {
            return $"[{Id}] {Date} - {Description} | {Amount:C} ({Category})";
        }
    }
}