

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
        private const decimal DefaultInterestRate = 0.02m; // 2% per år
        public decimal InterestRate { get; private set; } = DefaultInterestRate;

        // Constructor
        public BankAccount(string name, AccountType accountType, Currency currency, decimal initialBalance, DateTime? lastUpdated = null)
        {
            Name = name;
            AccountType = accountType;
            Currency = currency;
            Balance = initialBalance;
            LastUpdated = lastUpdated ?? DateTime.Now;
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
            if (amount < 0) throw new ArgumentException("Amount must be greater then 0!");
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
            if (amount < 0) throw new ArgumentException("Amount must be greater then 0!");
            if (Balance < amount) throw new InvalidOperationException("Insuficent balance");
            Balance -= amount;
            LastUpdated = DateTime.Now;
            _transaction.Add(new Transaction
            {
                transactionType = TransactionType.Withdraw,
                Amount = amount,
                BalanceAfterTransaction = Balance
            });
        }

        /// <summary>
        /// Calculates daily interest and applies if savings account
        /// </summary>
        public void ApplyInterest()
        {
            if (AccountType != AccountType.Savings)
                return;
            var daysElapsed = (DateTime.Now - LastUpdated).Days;
            if (daysElapsed <= 0) return;
            decimal dailyRate = InterestRate / 365m;
            decimal interestAmount = Balance * dailyRate * daysElapsed;
            Balance += Math.Round(interestAmount, 2);
            LastUpdated = DateTime.Now;
            _transaction.Add(new Transaction
            {
                transactionType = TransactionType.Interest,
                Amount = Math.Round(interestAmount, 2),
                BalanceAfterTransaction = Balance,
                TimeStamp = DateTime.Now
            });


        }
    }
}