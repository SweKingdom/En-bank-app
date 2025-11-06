using System.Security.Cryptography.X509Certificates;

namespace BlazorApp4.Domain
{
    /// <summary>
    /// Transaction domain, manages transactions, their types, time, account ID and curencys
    /// </summary>
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public TransactionType transactionType { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public Guid? FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }
    }
}