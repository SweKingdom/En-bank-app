namespace BlazorApp4.Domain;


/// <summary>
/// The different types of transactions
/// </summary>
public enum TransactionType
{
    Deposit,
    Withdraw,
    TransferIn,
    TransferOut,
    Interest
}

/// <summary>
/// The different account types
/// </summary>
public enum AccountType
{
    Savings,
    Deposit
}

/// <summary>
/// The different value types
/// </summary>
public enum Currency
{
    SEK,
    Euro,
    USD
}