
namespace BlazorApp4.Interfaces
{
    public interface IAccountService
    {
        IBankAccount CreateAccount(string name, Domain.AccountType accountType, Domain.Valuta currency, decimal initialBalance);


        List<IBankAccount> GetAccounts();
    }
}
