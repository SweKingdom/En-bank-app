namespace BlazorApp4.Domain.DTOs
{
    public class ImportedAccountData
    {
        public string Name { get; set; } = string.Empty;
        public int AccountType { get; set; }
        public int Currency { get; set; }
        public decimal Balance { get; set; }
        public List<ImportedTransaction> Transactions { get; set; } = new();

    }
}
