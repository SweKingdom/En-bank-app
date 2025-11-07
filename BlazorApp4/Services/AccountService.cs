
using System.Text.Json;

namespace BlazorApp4.Services
{
    /// <summary>
    /// Services responsible for managing bankaccount and transactions
    /// Handels storage, retrival and uppdates of account data
    /// </summary>
    public class AccountService : IAccountService, IDisposable
    {
        /// Private fields and lists
        private const string StorageKey = "BlazorApp4.accounts";
        private readonly List<BankAccount> _accounts = new();
        private readonly IStorageService _storageService;
        private bool isLoaded;
        private bool isRunning;
        private const string CorrectPin = "1234";

        public event Action? StateChanged;

        /// <summary>
        /// Triggers a manual state change notification.
        /// </summary>
        public void NotifyEvent()
        {
            StateChanged?.Invoke();
            Console.WriteLine("[AccountService] State change event triggered.");
        }

        /// Constructor
        public AccountService(IStorageService storageService)
        {
            _storageService = storageService;
            Console.WriteLine("[AccountService] Initialized.");
        }

        /// <summary>
        /// Ensure account list is loaded before use
        /// </summary>
        public async Task EnsureLoadedAsync()
        {
            if (isLoaded)
            {
                Console.WriteLine("[AccountService] Data already loaded; skipping reload.");
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
            Console.WriteLine($"[AccountService] Retrieved {fromStorage.Count} accounts from storage.");
        }

        /// <summary>
        /// Saves current account list to storage
        /// </summary>
        private Task SaveAsync() => _storageService.SetItemAsync(StorageKey, _accounts.OfType<BankAccount>().ToList());

        /// <summary>
        /// Creates and saves accounts with given parameters
        /// </summary>
        /// <param name="name">Account name</param>
        /// <param name="accountType">Type of account, savings, deposit</param>
        /// <param name="currency">What curency used, SEK</param>
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
                Console.WriteLine($"[AccountService] Account '{updatedAccount.Name}' updated successfully.");
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
                throw new InvalidOperationException("Insuficcent founds on from-account.");
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
            fromAccount.TransferTo(toAccount, amount);
            await SaveAsync();
            Console.WriteLine($"[AccountService] Transfer complete");
        }

        /// <summary>
        /// Deposits a specified amount into an account
        /// </summary>
        /// <param name="accountId">Unique account ID</param>
        /// <param name="amount">Amount depositid in to account</param>
        /// <exception cref="KeyNotFoundException">Unable to find Account ID</exception>
        /// <exception cref="ArgumentOutOfRangeException">Amount must be positive when depositing</exception>
        public async Task DepositAsync(Guid accountId, decimal amount)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId)
                ?? throw new KeyNotFoundException($"Account with ID {accountId} not found.");
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
            account.Deposit(amount);
            await SaveAsync();
            Console.WriteLine($"[AccountService] Deposit: {amount} to {account.Name}");
        }

        /// <summary>
        /// Withdraws a specified amount from an account
        /// </summary>
        /// <param name="accountId">Unique account ID</param>
        /// <param name="amount">Amount withdrawn from account</param>
        /// <exception cref="KeyNotFoundException">Unable to find Account ID</exception>
        /// <exception cref="ArgumentOutOfRangeException">Amount when withdrawn must be positive</exception>
        /// <exception cref="InvalidOperationException">Insufficent balance when withdrawing amount</exception>
        public async Task WithdrawAsync(Guid accountId, decimal amount)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId)
                ?? throw new KeyNotFoundException($"Account with ID {accountId} not found.");
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");
            if (account.Balance < amount)
                throw new InvalidOperationException("Insufficient balance.");
            account.Withdraw(amount);
            await SaveAsync();
            Console.WriteLine($"[AccountService] Withdraw: {amount} from {account.Name}");
        }

        /// <summary>
        /// Applies interest to all savings accounts
        /// </summary>
        public async Task ApplyInterestToSavingsAccounts()
        {
            foreach (var account in _accounts.Where(a => a.AccountType == AccountType.Savings))
            {
                account.ApplyInterest();
            }
            await SaveAsync();
            Console.WriteLine("[AccountService] Applyinterest Manualy");
        }

        /// <summary>
        /// Automatically applies daily interest if days have passed since last update
        /// </summary>
        public async Task ApplyDailyInterestAsync()
        {
            foreach (var account in _accounts.Where(a => a.AccountType == AccountType.Savings))
            {
                Console.WriteLine($"Checks interest from {account}, days since last update: {(DateTime.Now - account.LastUpdated).Days}");
                var daysElapsed = (DateTime.Now - account.LastUpdated).Seconds;
                if (daysElapsed > 0)
                {
                    account.ApplyInterest();
                }
            }
            await SaveAsync();
            NotifyEvent();
            Console.WriteLine("[AccountService] ApplyDailyInterestAsync");
        }

        public void AutoApplyDailyInterest()
        {
            isRunning = true;
            Task.Run(async () => {
                while (isRunning == true)
                {
                    await Task.Delay(5000);
                    await ApplyDailyInterestAsync();
                    Console.WriteLine("[AccountService] Auto ApplyDailyInterestAsync check");
                }
            });
        }



        /// <summary>
        /// Validates a user PIN asynchronously
        /// </summary>
        /// <param name="pin">PIN to validate</param>
        /// <returns>True if valid, otherwise false</returns>
        public Task<bool> ValidatePinAsync(string pin) => Task.FromResult(pin == CorrectPin);

        /// <summary>
        /// Exports all transactions of a specific account to a JSON file
        /// </summary>
        /// <param name="accountId">Unique account ID</param>
        public async Task ExportTransactionsAsync(Guid accountId)
        {
            await EnsureLoadedAsync();
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                Console.WriteLine($"[AccountService] Account {accountId} not found");
                return;
            }
            var exportData = new // Format på filen
            {
                account.Id,
                account.Name,
                account.AccountType,
                account.Currency,
                account.Balance,
                account.LastUpdated,
                Transactions = account.Transactions.Select(t => new
                {
                    t.TimeStamp,
                    t.Amount,
                    t.transactionType,
                    t.BalanceAfterTransaction,
                    FromAccount = t.FromAccountId,
                    ToAccount = t.ToAccountId,
                    t.Currency
                })
            };
            var json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions// Converts C# obj -> JSON string
            {
                WriteIndented = true
            });
            await _storageService.DownloadFileAsync($"{account.Name}_transactions.json", json); // Downloading
        }

        /// <summary>
        /// Downloads a file using the storage service
        /// </summary>
        /// <param name="fileName">File name downloaded</param>
        /// <param name="content">content in file</param>
        public async Task DownloadFileAsync(string fileName, string content)
        {
            await _storageService.DownloadFileAsync(fileName, content);
        }

        /// <summary>
        /// Imports transactions from a JSON file and adds a new account
        /// </summary>
        public async Task ImportTransactionAsync()
        {
            try
            {
                Console.WriteLine("[AccountService] Importing JSON");
                var json = await _storageService.ReadFileAsync(); // Reads file through StorageService
                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("[AccountService] No file content read.");
                    return;
                }
                var importData = JsonSerializer.Deserialize<ImportedAccountData>(json); // Deserialized JSON to C# obj
                if (importData == null)
                {
                    Console.WriteLine("[AccountService] Failed to deserialize JSON.");
                    return;
                }
                var newAccount = new BankAccount(
                    importData.Id == Guid.Empty ? Guid.NewGuid() : importData.Id,
                    importData.Name,
                    (AccountType)importData.AccountType,
                    (Currency)importData.Currency,
                    importData.Balance,
                    importData.LastUpdated
                );
                foreach (var t in importData.Transactions)
                {
                    newAccount.Transactions.Add(new Transaction
                    {
                        Id = t.Id,
                        TimeStamp = t.TimeStamp,
                        Amount = t.Amount,
                        transactionType = (TransactionType)t.transactionType,
                        BalanceAfterTransaction = t.BalanceAfterTransaction,
                        FromAccountId = t.FromAccount,
                        ToAccountId = t.ToAccount,
                        Currency = (Currency)t.Currency
                    });
                }
                _accounts.Add(newAccount);
                await SaveAsync();
                Console.WriteLine($"[AccountService] Imported account '{newAccount.Name}' with {newAccount.Transactions.Count} transactions.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AccountService] Error importing: {ex.Message}");
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
