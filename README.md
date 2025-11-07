The is a Blazor WebAssembly Project I built to practice C# and client side web development. It simulates a basic banking system where you can create accounts, make deposits, withdrawls and transfers. All data is stored locally in the browser using LocalStorage, there for your accounts and transactions remains after refreshing or closing the page.



--VG Additions--

1. A PIN code lock with a 4-digit PIN to unlock the app has been added.
2. I have chosen to add Three additions to my project that is required for a higher grade.
   2% interest that automaticly applies if the account is a savings account. This works on imported accounts. Can also manually add interest to savings account.
3. JSON Import/Export makes it possible for users to export all accounts and transactions to JSON files, and import data. Added validation to not be able to import same account if one account already have the same Guid.





--Getting Started--

Default PIN = 1234
Change PIN: Open Services/AccountService.cs and eddit the constant CorrectPin.



--Prerequisites--

.NET 8 SDK

Visual Studio 2022 (17.8+) with ASP.NET and web development workload

A modern browser (Edge/Chrome/Firefox/Safari etc...)



-- Run the application --

1. Open Console App and navigate with cd to your map containing your program files
2. Clone the repository: git clone https://github.com/SweKingdom/En-bank-app.git
3. Open the program by running the BlazorApp4.sln program
4. Run the application: Start Without Debugging (Ctrl+F5)
5. The browser opens automatically. If not, copy the URL from the Output window and paste it manually.



-- Features --

PIN Lock

* A White startup screen that requires a pin to acces the app.
* Default PIN = 1234 (can be changed in code).



Accounts

* Create new accounts with a name, type (Salary or Savings), currency (SEK), and an opening balance (> 0).
* Each account has its own transaction history.



Transactions

* Deposit, withdraw, or transfer money between accounts.
* Transfers require two different accounts.
* Amounts must be greater than 0.
* Withdrawals cannot exceed available balance.



Interest on Savings

* Savings accounts can have an interest rate applied manually or automatically.
* The app can simulate daily interest through a timer.



Import / Export JSON

* Export all account data to a JSON file for backup.
* Import JSON files to import accounts.
* Uses browser file dialogs via JS interop.



Transaction History

* Shows all transactions for each account.
* Includes timestamp, type, amount, and updated balance.



-- Project Structure --

Domain/ – logic classes like BankAccount, Transaction, InterestCalculator.

Services/ – handles app logic and data persistence (AccountService, StorageService, LockService).

Interfaces/ – defines how services should behave (e.g., IAccountService, IStorageService).

Pages/ – Blazor components for UI (Home.razor, CreateAccount.razor, NewTransaction.razor).

Layout/ – layout and navigation (NoNavLayout.razor).

Program.cs – dependency injection setup and startup configuration.









-- Pages --

Home => Login screen (PIN entry).

CreateAccount => Create new account.

NewTransaction => Deposit / Withdraw / Transfer.

History (future) => Transaction list for accounts.



\- Validation Rules --

Create Account

* Name is required.
* Opening balance must be ≥ 0.



Transactions

* Amount must be > 0.
* Transfer: accounts must be different.
* Withdraw: cannot exceed account balance.
* Validation errors are shown in the UI.



-- Data Storage --

All data is saved as JSON inside your browser’s localStorage under the key:

BlazorApp4.accounts.

To reset all data, clear your browser’s localStorage.



-- Common Issues --

If the app doesn’t load correctly, try clearing localStorage (old data may conflict after updates).

If navigation or @onclick handlers break, make sure string routes are wrapped in quotes.

If JSON import fails, verify the file format matches the current data structure.



-- Future Improvements --



A key future imporvment would be to use a real database instead of LocalStorage. Since this is a small simulation and i have not learned how to work with backend APIs or databases, all data is currently stored locally in the browser.



Ading real user authentication would make the app more secure and improved UI feedback could provide a smoother user experience.

Support for multiple currencies and multiple languages would make the system more flexible and accessible.

Finally, instead of using Console.WriteLine() statements, loggs could be handled through the ILogger interface to enable structured logging and better debugging in the future.

Something to revisit.

