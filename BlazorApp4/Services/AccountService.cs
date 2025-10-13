

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BlazorApp4.Services
{
    public class AccountService : IAccountService
    {


        private const string StorageKey = "bankapp.accounts";
        
        private readonly List<BankAccount> _accounts = new();

        private readonly IStorageService _storageService;

        private readonly List<Transaction> _transactions = new();


        private bool isLoaded;





        public AccountService(IStorageService storageService) => _storageService = storageService;


        private async Task IsInitialized()
        {
            if(isLoaded)
            {
                return;
            }

            var fromStorage = await _storageService.GetItemsAsync<List<BankAccount>>(StorageKey);
            _accounts.Clear();
            if (fromStorage is {Count: > 0 })
            
                _accounts.AddRange(fromStorage);
            isLoaded = true;
            
        }


        private Task SaveAsync() => _storageService.SetItemAsync(StorageKey, _accounts);


        public async Task<IBankAccount> CreateAccount(string name, Domain.AccountType accountType, Currency currency, decimal initialBalance)
        {
            await IsInitialized();
            var account = new BankAccount(name, accountType, currency, initialBalance);
            _accounts.Add(account);
            await SaveAsync();
            return account;
        }

        public async Task<List<IBankAccount>> GetAccounts()
        {
            await IsInitialized();
            return _accounts.Cast<IBankAccount>().ToList();
        }

        public List<Transaction> GetTransactions() => _transactions;


    public void Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            if (amount == 0)
            {
                throw new ArgumentException("Belloppet måste vara större än 0");
            }

            var fromAccount = _accounts.FirstOrDefault(a => a.Id == fromAccountId);
            var toAccount = _accounts.FirstOrDefault(a => a.Id == toAccountId);

            if (fromAccount == null || toAccount == null)
            {
                throw new ArgumentException("Ett eller båda konton hittades inte");
            }

            if (fromAccount.Balance < amount)
            {
                throw new ArgumentException("Otilräckligt saldo på avsändarkontot");
            }

            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);

            var transaction = new Transaction(fromAccountId, toAccountId, amount);
            _transactions.Add(transaction);
        }
        }
}
