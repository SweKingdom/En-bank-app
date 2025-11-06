namespace BlazorApp4.Domain.DTOs
{
    /// <summary>
    /// Data for an imported account, included its details and transactions
    /// </summary>
    public class ImportedAccountData
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AccountType { get; set; }
        public int Currency { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<ImportedTransaction> Transactions { get; set; } = new();

    }
}
