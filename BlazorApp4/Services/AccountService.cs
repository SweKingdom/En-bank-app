

using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BlazorApp4.Services
{
    public class AccountService : IAccountService
    {


        private const string StorageKey = "bankapp.accounts";
        private const string TransactionsKey = "bankapp.transactions";

        private readonly List<BankAccount> _accounts = new();
        private readonly List<Transaction> _transactions = new();

        private readonly IStorageService _storageService;




        private bool isLoaded;
        private bool transactionsLoaded;



        /// <summary>
        /// Laddar konton från localStorage
        /// </summary>
        public AccountService(IStorageService storageService) => _storageService = storageService;


        private async Task IsInitialized()
        {
            if (isLoaded)
            {
                return;
            }

            var fromStorage = await _storageService.GetItemsAsync<List<BankAccount>>(StorageKey);
            _accounts.Clear();
            if (fromStorage is { Count: > 0 })

                _accounts.AddRange(fromStorage);
            isLoaded = true;

        }


        /// <summary>
        /// Laddar transaktioner från localStorage
        /// </summary>
        private async Task InitializeTransactionsAsync()
        {
            if (transactionsLoaded)
            {
                return;
            }
            var fromStorage = await _storageService.GetItemsAsync<List<Transaction>>(TransactionsKey);
            _transactions.Clear();
            if (fromStorage is { Count: > 0 })
                _transactions.AddRange(fromStorage);

            transactionsLoaded = true;
        }



        private Task SaveAsync() => _storageService.SetItemAsync(StorageKey, _accounts);


        private Task SaveTransactionsAsync() => _storageService.SetItemAsync(TransactionsKey, _transactions);


        public async Task<IBankAccount> CreateAccount(string name, Domain.AccountType accountType, Currency currency, decimal initialBalance)
        {
            await IsInitialized();
            var account = new BankAccount(name, accountType, currency, initialBalance);
            _accounts.Add(account);
            await SaveAsync();
            return account;
        }


        /// <summary>
        /// Hämtar alla konton
        /// </summary>


        public async Task<List<IBankAccount>> GetAccounts()
        {
            await IsInitialized();
            return _accounts.Cast<IBankAccount>().ToList();
        }


        /// <summary>
        /// Hämtar alla Transaktioner
        /// </summary>
        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            await InitializeTransactionsAsync();
            return _transactions;
        }



        /// <summary>
        /// Överföring 
        /// </summary>
        public async Task Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            await IsInitialized();
            await InitializeTransactionsAsync();

            if (amount <= 0)
                throw new ArgumentException("Beloppet måste vara större än 0");

            var fromAccount = _accounts.FirstOrDefault(a => a.Id == fromAccountId);
            var toAccount = _accounts.FirstOrDefault(a => a.Id == toAccountId);

            if (fromAccount == null || toAccount == null)
                throw new ArgumentException("Ett eller flera konton hittades inte");

            if (fromAccount.Balance < amount)
                throw new ArgumentException("Otillräckligt saldo på avsändarkontot");

            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);

            // Skapa transaktioner
            var transferOut = new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                TimeStamp = DateTime.Now,
                Type = TransactionType.TransferOut
            };

            var transferIn = new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                TimeStamp = DateTime.Now,
                Type = TransactionType.TransferIn
            };

            _transactions.Add(transferOut);
            _transactions.Add(transferIn);

            await SaveAsync();
            await SaveTransactionsAsync();
        }




    }
}
