

namespace BlazorApp4.Domain
{
    public class BankAccount : IBankAccount
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; private set; }

        public string Type { get; private set; }

        public string Currency { get; private set; }

        public decimal Balance { get; private set; }

        public DateTime LastUpdated { get; private set; }

        public BankAccount(string name, string type, string currency, decimal balance)
        {
            Name = name;
            Type = type;
            Currency = currency;
            Balance = balance;
            LastUpdated = DateTime.Now;
        }




        public void Deposit(decimal amount)
        {
            
            if (amount < 0) { throw new NotImplementedException(); }

            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount < 0) { throw new NotImplementedException(); }

            Balance -= amount;
        }
    }
}
