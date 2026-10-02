using GestoreSpeseBudget.Models;
using System.Text.Json;

namespace GestoreSpeseBudget.Repositories
{
    internal class JsonExpenseRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public JsonExpenseRepository()
        {
            _filePath = "expenses.json";
            _options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
        }

        public void Save(IReadOnlyList<Expense> expenses)
        {
            string jsonString = JsonSerializer.Serialize(expenses, _options);
            File.WriteAllText(_filePath, jsonString);
        }

        public List<Expense> Load()
        {
            if (!File.Exists(_filePath))
                return [];

            string jsonString = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(jsonString))
                return [];

            return JsonSerializer.Deserialize<List<Expense>>(jsonString, _options) ?? [];
        }
    }
}