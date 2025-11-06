namespace BlazorApp4.Domain.DTOs
{
    /// <summary>
    /// Transactions and its transaction details from a imported account
    /// </summary>
    public class ImportedTransaction
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Amount { get; set; }
        public int transactionType { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public Guid? FromAccount { get; set; }
        public Guid? ToAccount { get; set; }
        public int Currency { get; set; }

    }
}
