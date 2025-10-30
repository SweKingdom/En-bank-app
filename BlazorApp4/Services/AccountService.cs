
namespace BlazorApp4.Services
{
    /// <summary>
    /// Services responsible for managing bankaccount and transactions
    /// Handels storage, retrival and uppdates of account data
    /// </summary>
    public class AccountService : IAccountService
    {
        
        /// Private fields and lists
        private const string StorageKey = "BlazorApp4.accounts";
        private readonly List<BankAccount> _accounts = new();
        private readonly IStorageService _storageService;
        private bool isLoaded;

        /// Constructor
        public AccountService(IStorageService storageService)
        {
            _storageService = storageService;

        }

        /// <summary>
        /// Ensure account list is loaded before use
        /// </summary>
        public async Task EnsureLoadedAsync()
        {
            if (isLoaded)
            {
                return;
            }
            await IsInitialized();
            isLoaded = true;
        }

        /// <summary>
        /// Loads account data from storage
        /// </summary>
        private async Task IsInitialized()
        {
            var fromStorage = await _storageService.GetItemAsync<List<BankAccount>>(StorageKey);
            if (fromStorage is { Count: > 0 })
                _accounts.AddRange(fromStorage);
            isLoaded = true;
        }
        /// <summary>
        /// Saves current account list to storage
        /// </summary>
        private Task SaveAsync() => _storageService.SetItemAsync(StorageKey, _accounts.OfType<BankAccount>().ToList());

        /// <summary>
        /// Creates and saves accounts with given parameters
        /// </summary>
        /// <param name="name">Account name</param>
        /// <param name="accountType">Tyoe of account, savings... deposit</param>
        /// <param name="currency">What curency used, sek default</param>
        /// <param name="initialBalance">Account balance upon created</param>
        /// <returns>Returns created instans of bankaccount</returns>
        public async Task<BankAccount> CreateAccount(string name, AccountType accountType, Currency currency, decimal initialBalance)
        {
            var account = new BankAccount(name, accountType, currency, initialBalance);
            _accounts.Add(account);
            await SaveAsync();
            return account;
        }

        /// <summary>
        /// Returns all curently loaded bankaccounts
        /// </summary>
        /// <returns>List of bank accounts</returns>
        public List<BankAccount> GetAccounts()
        {
            return _accounts.Cast<BankAccount>().ToList();
        }

        /// <summary>
        /// Deletes account by its uniq ID
        /// </summary>
        /// <param name="Id">Uniqe account ID</param>
        public async Task DeleteAccount(Guid Id)
        {
            var accountToRemove = _accounts.FirstOrDefault(account => account.Id == Id);
            if (accountToRemove is not null)
            {
                _accounts.Remove(accountToRemove);
                await SaveAsync();
            }
        }

        /// <summary>
        /// Uppdates a excisting account by replacing it with a updated instance
        /// </summary>
        /// <param name="updatedAccount">New instance of account</param>
        public async Task UpdateAccount(BankAccount updatedAccount)
        {
            var existing = _accounts.FirstOrDefault(account => account.Id == updatedAccount.Id);
            if (existing != null)
            {
                _accounts.Remove(existing);
                _accounts.Add(updatedAccount);
                await SaveAsync();
            }
        }

        /// <summary>
        /// Transfers amount between two different accounts and updates the balances
        /// </summary>
        /// <param name="fromAccountId">The ID of the source amount</param>
        /// <param name="toAccountId">ID of the recipitant account</param>
        /// <param name="amount">Amount tranfered from account to account</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Thrown if either account is not found</exception>
        /// <exception cref="InvalidOperationException">Thrown if insufficent founds are available</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if tranfer amount is negative</exception>
        public async Task Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var fromAccount = _accounts.OfType<BankAccount>().FirstOrDefault(a => a.Id == fromAccountId)
            ?? throw new KeyNotFoundException($"Account with ID {fromAccountId} not found.");
            var toAccount = _accounts.OfType<BankAccount>().FirstOrDefault(a => a.Id == toAccountId)
            ?? throw new KeyNotFoundException($"Account with ID {toAccountId} not found.");

            if (fromAccount.Balance < amount)
                throw new InvalidOperationException("Otillräckliga medel på från-kontot.");
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Beloppet måste vara positivt.");
            fromAccount.TransferTo(toAccount, amount);

            await SaveAsync();
        }


    }
}
