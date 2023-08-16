using System;
using System.Text;
using System.Linq;
using System.Transactions;

namespace MyBank
{
    public class BankAccount
    {

        public string Number { get; }

        public string Owner { get; set; }

        public bool ShowMenu { get; set; }

        public decimal Balance {
            get
            {
                // sets original balance to 0
                decimal balance = 0;
                // goes through all transactions
                foreach (var item in allTransactions)
                {
                    balance += item.Amount;
                }
                return balance;
            }
        }

        private static int accountNumberSeed = 1234567890;

        private List<Transaction> allTransactions = new List<Transaction>();

        public BankAccount(string name, decimal initialBalance)
        {
            this.Owner = name;
            // first transaction is the initial balance
            MakeDeposit(initialBalance, DateTime.Now, "Initial Balance");
            this.Number = accountNumberSeed.ToString();
            accountNumberSeed++;
        }

        // should an 'out' be
        public void Menu()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Check Balance");
            Console.WriteLine("2. Make a Deposit");
            Console.WriteLine("3. Make a Withdrawl");
            Console.WriteLine("4. Get Account History");
        }

        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            // if you try to deposit negative amount
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
            }
            var deposit = new Transaction(amount, date, note);
            allTransactions.Add(deposit);
        }

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }

            // if the balance minus the amount to withdraw would result in less than 0
            if (Balance - amount < 0)
            {
                throw new InvalidOperationException("Not sufficient funds for this withdrawal");
            }
            var withdrawal = new Transaction(-amount, date, note);
            allTransactions.Add(withdrawal);
        }

        public string GetAccountHistory()
        {
            // report is a StringBuilder type
            var report = new StringBuilder();

            // Creates Header
            report.AppendLine("Date\t\tAmount\tNote");
            foreach (var item in allTransactions)
            {
                // Creates Row
                report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{item.Notes}");
            }
            // As report is of StringBuilder Type, this needs to convert to String
            return report.ToString();
        }

        public void EndOfService()
        {
            for (int i = 0; i < 50; i++)
            {
                Console.Write(".");
            }
        }
    }
}

