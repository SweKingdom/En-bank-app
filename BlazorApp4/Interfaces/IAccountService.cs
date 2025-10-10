
namespace BlazorApp4.Interfaces
{
    public interface IAccountService
    {
        IBankAccount CreateAccount(string name, Domain.AccountType accountType, Currency currency, decimal initialBalance);


        List<IBankAccount> GetAccounts();
    }
}
