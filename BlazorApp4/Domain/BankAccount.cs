

using System.Text.Json.Serialization;

namespace BlazorApp4.Domain
{
    public class BankAccount : IBankAccount
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; private set; }

        public AccountType AccountType { get; private set; }

        public Currency Currency { get; private set; }

        public decimal Balance { get; private set; }

        public DateTime LastUpdated { get; private set; }



        private readonly List<Transaction> _transactions = new();
        public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly(); //tabort?




        public BankAccount(string name, AccountType accountType, Currency currency, decimal initialBalance)
        {
            Name = name;
            AccountType = accountType;
            Currency = currency;
            Balance = initialBalance;
            LastUpdated = DateTime.Now;
        }

        [JsonConstructor]
        public BankAccount(Guid id, string name, AccountType accountType, Currency currency, decimal balance, DateTime lastUpdated)
        {
            Id = id;
            Name = name;
            AccountType = accountType;
            Currency = currency;
            Balance = balance;
            LastUpdated = lastUpdated;
        }




        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Beloppet måste vara större än 0.");

            Balance += amount;
            LastUpdated = DateTime.Now;

            _transactions.Add(new Transaction
            {
                ToAccountId = Id,
                Amount = amount,
                TimeStamp = DateTime.Now,
                Type = TransactionType.Deposit
            });
        }


        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Beloppet måste vara större än 0.");

            if (amount > Balance)
                throw new InvalidOperationException("Otillräckligt saldo.");

            Balance -= amount;
            LastUpdated = DateTime.Now;

            _transactions.Add(new Transaction
            {
                FromAccountId = Id,
                Amount = amount,
                TimeStamp = DateTime.Now,
                Type = TransactionType.Withdraw
            });
        }

        public void TransferTo(BankAccount toAccount, decimal amount)
        {
            //Från vilket konto
            Balance -= amount;
            LastUpdated = DateTime.UtcNow;
            _transactions.Add(new Transaction
            {
                Type = TransactionType.TransferOut,
                Amount = amount,
                BalanceAfter = Balance,
                FromAccountId = Id,
                ToAccountId = toAccount.Id,

            });

            //Till vilketkonmto

            toAccount.Balance += amount;
            toAccount.LastUpdated = DateTime.UtcNow;
            toAccount._transactions.Add(new Transaction
            {
                Type = TransactionType.TransferIn,
                Amount = amount,
                BalanceAfter = Balance,
                FromAccountId = Id,
                ToAccountId = toAccount.Id,
            });
        }
    }
}
