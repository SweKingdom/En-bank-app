

using System.Text.Json.Serialization;

namespace BlazorApp4.Domain
{
    /// <summary>
    /// BankAccount domain, manages transactions, transfers and saves properties connected to the bankaccount
    /// </summary>
    public class BankAccount : IBankAccount
    {
        // Constants
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public AccountType AccountType { get; private set; }
        public Currency Currency { get; private set; }
        public decimal Balance { get; private set; }
        public DateTime LastUpdated { get; private set; }
        public readonly List<Transaction> _transaction = new();
        public List<Transaction> Transactions => _transaction;

        // Constructor
        public BankAccount(string name, AccountType accountType, Currency currency, decimal initialBalance)
        {
            Name = name;
            AccountType = accountType;
            Currency = currency;
            Balance = initialBalance;
            LastUpdated = DateTime.Now;
        }

        [JsonConstructor]
        public BankAccount(Guid id, string name, AccountType accountType, Currency currency, decimal balance, DateTime lastUpdated, List<Transaction>? transactions = null)
        {
            Id = id;
            Name = name;
            AccountType = accountType;
            Currency = currency;
            Balance = balance;
            LastUpdated = lastUpdated;

            if (transactions != null)
                _transaction = transactions;
        }

        /// <summary>
        /// Transfer a specific amountfrom one account to another
        /// </summary>
        /// <param name="toAccount">Which account to transfer to</param>
        /// <param name="amount">The specified amount transfered</param>
        public void TransferTo(BankAccount toAccount, decimal amount)
        {
            // From what account
            Balance -= amount;
            LastUpdated = DateTime.Now;
            _transaction.Add(new Transaction
            {
                transactionType = TransactionType.TransferOut,
                Amount = amount,
                BalanceAfterTransaction = Balance,
                FromAccountId = Id,
                ToAccountId = toAccount.Id,
                TimeStamp = DateTime.Now
            });

            // To what account
            toAccount.Balance += amount;
            toAccount.LastUpdated = DateTime.Now;
            toAccount._transaction.Add(new Transaction
            {
                transactionType = TransactionType.TransferIn,
                Amount = amount,
                BalanceAfterTransaction = Balance,
                FromAccountId = Id,
                ToAccountId = toAccount.Id,
                TimeStamp = DateTime.Now
            });
        }

        /// <summary>
        /// Deposit a specific amount to the bankaccount balance
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Deposit(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("Beloppet måste vara större än 0!");
            Balance += amount;
            LastUpdated = DateTime.Now;

            _transaction.Add(new Transaction
            {
                transactionType = TransactionType.Deposit,
                Amount = amount,
                BalanceAfterTransaction = Balance
            });
        }

        /// <summary>
        /// Withdraw a specific amount from the bankaccount balance
        /// </summary>
        /// <param name="amount">Thespecified amount</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException">Cant have a negative balance</exception>
        public void Withdraw(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("Beloppet måste vara större än 0!");

            if (Balance < amount) throw new InvalidOperationException("Inte tillräckligt saldo!");
            Balance -= amount;
            LastUpdated = DateTime.Now;

            _transaction.Add(new Transaction
            {
                transactionType = TransactionType.Withdraw,
                Amount = amount,
                BalanceAfterTransaction = Balance
            });
        }
    }
}