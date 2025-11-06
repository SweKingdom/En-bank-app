namespace BlazorApp4.Interfaces;

/// <summary>
/// Defines the core operations and properties for a bank account, including transactions, deposits, withdrawals, and transfers.
/// </summary>
public interface IBankAccount
{
    //Properites
    Guid Id { get; }
    string Name { get; }
    AccountType AccountType { get; }
    Currency Currency { get; }
    decimal Balance { get; }
    DateTime LastUpdated { get; }
    List<Transaction> Transactions { get; }

    /// <summary>
    /// Withdraws a specific amount from the account balance
    /// </summary>
    /// <param name="amount">The amount to withdraw</param>
    void Withdraw(decimal amount);

    /// <summary>
    /// Deposits a specific amount into the account balance
    /// </summary>
    /// <param name="amount">The amount to deposit</param>
    void Deposit(decimal amount);

    /// <summary>
    /// Transfers a specified amount from this account to another bank account
    /// </summary>
    /// <param name="toAccount">The destination account to transfer funds to</param>
    /// <param name="amount">The amount to transfer</param>
    void TransferTo(BankAccount toAccount, decimal amount);
}