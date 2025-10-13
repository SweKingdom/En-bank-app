namespace BlazorApp4.Domain
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date {  get; set; }

        public Transaction(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Date = DateTime.Now;
        }
    }
}
