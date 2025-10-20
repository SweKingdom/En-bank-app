namespace BlazorApp4.Domain
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime TimeStamp { get; set; }
        public TransactionType Type { get; set; } // <-- använd kort "Type"

        // Parameterlös konstruktor (stöd för object initializers)
        public Transaction()
        {
            TimeStamp = DateTime.Now;
        }

        // Praktisk konstruktor du kan använda direkt
        public Transaction(Guid? fromAccountId, Guid? toAccountId, decimal amount, TransactionType type)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Type = type;
            TimeStamp = DateTime.Now;
        }
    }
}
