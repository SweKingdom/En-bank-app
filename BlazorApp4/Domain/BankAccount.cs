﻿

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
