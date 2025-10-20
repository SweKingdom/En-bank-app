namespace BlazorApp4.Domain;



public enum TransactionType
{
    Deposit,
    Withdraw,
    TransferIn,
    TransferOut
}

public enum AccountType
{
    Savings,
    Deposit
}

public enum Currency
{
    SEK,
    Euro,
    USD
}