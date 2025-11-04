namespace BlazorApp4.Domain.DTOs
{
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
