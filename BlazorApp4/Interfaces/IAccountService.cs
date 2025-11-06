
namespace BlazorApp4.Interfaces
{
    /// <summary>
    /// A interface for AccountService, defines methods for creating, managing and uppdating accounts
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Creates a new bank account with these specified details
        /// </summary>
        /// <param name="name">The name of the account</param>
        /// <param name="accountType">The type of account, savings, debit</param>
        /// <param name="currency">The currency being used by the account</param>
        /// <param name="initialBalance">The staring balance of the account</param>
        /// <returns>A newly created account instance</returns>
        Task<BankAccount> CreateAccount(string name, AccountType accountType, Currency currency, decimal initialBalance);
       
        /// <summary>
        /// Retreaves a list of all available accounts
        /// </summary>
        /// <returns>A list of objects</returns>
        List<BankAccount> GetAccounts();

        /// <summary>
        /// Deletes a bank account, depended on its unique ID
        /// </summary>
        /// <param name="Id">The unique ID of a account</param>
        Task DeleteAccount(Guid Id);

        /// <summary>
        /// Updates a existing account with new information
        /// </summary>
        /// <param name="updatedAccount">The account containing the updated details</param>
        /// <returns>Updated account</returns>
        Task UpdateAccount(BankAccount updatedAccount);

        /// <summary>
        /// Transfers a amount from balance from a account to another
        /// </summary>
        /// <param name="fromAccountId">Unique Id for the dispencing account</param>
        /// <param name="toAccountId">Unique Id for the receving account</param>
        /// <param name="amount">The amount transfered</param>
        Task Transfer(Guid fromAccountId, Guid toAccountId, decimal amount);

        /// <summary>
        /// Deposit a amount in to a account
        /// </summary>
        /// <param name="accountId">Unique Id for the account receving the deposit</param>
        /// <param name="amount">Amount deposit in to account</param>
        Task DepositAsync(Guid accountId, decimal amount);

        /// <summary>
        /// Withdraws a amount from a account
        /// </summary>
        /// <param name="accountId">Unique Id for the account withdrawing from</param>
        /// <param name="amount">Amount withdrawed from account</param>
        Task WithdrawAsync(Guid accountId, decimal amount);

        /// <summary>
        /// Ensures all account data and transactions are loaded before performing operations
        /// </summary>
        Task EnsureLoadedAsync();

        /// <summary>
        /// Applies interest to savings account
        /// </summary>
        Task ApplyInterestToSavingsAccounts();

        /// <summary>
        /// Validates a provided PIN for app access
        /// </summary>
        /// <param name="pin">Login code</param>
        Task<bool> ValidatePinAsync(string pin);

        /// <summary>
        /// Exports all transactions associated with a given account
        /// </summary>
        /// <param name="accountId">Unique account ID</param>
        Task ExportTransactionsAsync(Guid accountId);

        /// <summary>
        /// Imports transactions from an external source into the system.
        /// </summary>
        Task ImportTransactionAsync();

        event Action? StateChanged;

        void AutoApplyDailyInterest();
    }
}