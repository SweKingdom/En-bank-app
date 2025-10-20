
namespace BlazorApp4.Interfaces
{
    public interface IAccountService
    {
        Task <IBankAccount> CreateAccount(string name, Domain.AccountType accountType, Currency currency, decimal initialBalance);


        Task<List<IBankAccount>> GetAccounts();

        Task Transfer(Guid fromAccountId, Guid ToAccountId, decimal amount);

        Task<List<Transaction>> GetTransactionsAsync();
        

    }
}
