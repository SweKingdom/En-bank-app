namespace BlazorApp4.Interfaces;

/// <summary>
/// Interface containing the BankAccount methods
/// </summary>




public interface IBankAccount
{
    Guid Id { get; }
    string Name { get; }
    Domain.AccountType AccountType { get; }
    Valuta Currency {  get; }
    decimal Balance { get; }
    DateTime LastUpdated { get; }

    void Withdraw(decimal amount);
    void Deposit(decimal amount);
}