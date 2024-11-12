using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleBankingApp
{
    // User class to represent each user with a username, password, and list of accounts
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    // Account class to represent each bank account with a unique account number, type, balance, and transactions
    public class Account
    {
        private static int accountCounter = 1; // Unique account number counter
        public int AccountNumber { get; }
        public string AccountHolderName { get; }
        public string AccountType { get; }
        public decimal Balance { get; private set; }
        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public Account(string accountHolderName, string accountType, decimal initialDeposit)
        {
            AccountNumber = accountCounter++;
            AccountHolderName = accountHolderName;
            AccountType = accountType;
            Balance = initialDeposit;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            Transactions.Add(new Transaction(Transaction.GenerateTransactionId(), DateTime.Now, "Deposit", amount));
            Console.WriteLine($"Deposited: {amount}");
        }

        public void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                Transactions.Add(new Transaction(Transaction.GenerateTransactionId(), DateTime.Now, "Withdrawal", amount));
                Console.WriteLine($"Withdrew: {amount}");
            }
            else
            {
                Console.WriteLine("Insufficient funds.");
            }
        }

        public void GenerateStatement()
        {
            Console.WriteLine($"Statement for Account {AccountNumber}");
            foreach (var transaction in Transactions)
            {
                Console.WriteLine($"{transaction.Date} - {transaction.Type}: {transaction.Amount}");
            }
        }

        public void CalculateInterest()
        {
            if (AccountType.ToLower() == "savings")
            {
                decimal interest = Balance * 0.04m; // Fixed interest rate for simplicity
                Deposit(interest);
                Console.WriteLine("Monthly interest added.");
            }
        }

        public decimal CheckBalance()
        {
            return Balance;
        }
    }

    // Transaction class to store details of each transaction
    public class Transaction
    {
        private static int transactionCounter = 1;
        public int TransactionID { get; }
        public DateTime Date { get; }
        public string Type { get; }
        public decimal Amount { get; }

        public Transaction(int transactionId, DateTime date, string type, decimal amount)
        {
            TransactionID = transactionId;
            Date = date;
            Type = type;
            Amount = amount;
        }

        public static int GenerateTransactionId()
        {
            return transactionCounter++;
        }
    }

    // Main class to handle the banking application logic
    public class BankingApp
    {
        private List<User> users = new List<User>();

        public void Register(string username, string password)
        {
            var user = new User(username, password);
            users.Add(user);
            Console.WriteLine("Registration successful!");
        }

        public User Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                Console.WriteLine("Invalid credentials. Please try again.");
            }
            return user;
        }

        public void OpenAccount(User user, string accountHolderName, string accountType, decimal initialDeposit)
        {
            var account = new Account(accountHolderName, accountType, initialDeposit);
            user.Accounts.Add(account);
            Console.WriteLine("Account opened successfully!");
        }

        public void DisplayMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter username: ");
                        string username = Console.ReadLine();
                        Console.Write("Enter password: ");
                        string password = Console.ReadLine();
                        Register(username, password);
                        break;

                    case 2:
                        Console.Write("Enter username: ");
                        username = Console.ReadLine();
                        Console.Write("Enter password: ");
                        password = Console.ReadLine();
                        var user = Login(username, password);
                        if (user != null)
                        {
                            UserOperations(user);
                        }
                        break;

                    case 3:
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void UserOperations(User user)
        {
            bool loggedIn = true;
            while (loggedIn)
            {
                Console.WriteLine("\n1. Open Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Generate Statement");
                Console.WriteLine("5. Calculate Interest");
                Console.WriteLine("6. Check Balance");
                Console.WriteLine("7. Logout");
                Console.Write("Select an option: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter account holder name: ");
                        string accountHolderName = Console.ReadLine();
                        Console.Write("Enter account type (savings/checking): ");
                        string accountType = Console.ReadLine();
                        Console.Write("Enter initial deposit: ");
                        decimal initialDeposit = decimal.Parse(Console.ReadLine());
                        OpenAccount(user, accountHolderName, accountType, initialDeposit);
                        break;

                    case 2:
                        Console.Write("Enter account number: ");
                        int accountNumber = int.Parse(Console.ReadLine());
                        var account = user.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
                        if (account != null)
                        {
                            Console.Write("Enter amount to deposit: ");
                            decimal depositAmount = decimal.Parse(Console.ReadLine());
                            account.Deposit(depositAmount);
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;

                    case 3:
                        Console.Write("Enter account number: ");
                        accountNumber = int.Parse(Console.ReadLine());
                        account = user.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
                        if (account != null)
                        {
                            Console.Write("Enter amount to withdraw: ");
                            decimal withdrawalAmount = decimal.Parse(Console.ReadLine());
                            account.Withdraw(withdrawalAmount);
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;

                    case 4:
                        Console.Write("Enter account number: ");
                        accountNumber = int.Parse(Console.ReadLine());
                        account = user.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
                        if (account != null)
                        {
                            account.GenerateStatement();
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;

                    case 5:
                        Console.Write("Enter account number: ");
                        accountNumber = int.Parse(Console.ReadLine());
                        account = user.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
                        if (account != null)
                        {
                            account.CalculateInterest();
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;

                    case 6:
                        Console.Write("Enter account number: ");
                        accountNumber = int.Parse(Console.ReadLine());
                        account = user.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
                        if (account != null)
                        {
                            Console.WriteLine($"Balance: {account.CheckBalance()}");
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;

                    case 7:
                        loggedIn = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        public static void Main(string[] args)
        {
            var app = new BankingApp();
            app.DisplayMenu();
        }
    }
}
