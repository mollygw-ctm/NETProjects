using System;

namespace MyBank
{
    class Program
    {
        static void Main(string[] args)
        {
            // creates new bank account with £1000 inside
            Console.WriteLine("Would you like to create a bank account? Y/N");
            var userCreate = Console.ReadLine();
            Console.WriteLine("What is your name?");
            var userName = Console.ReadLine();


            var account = new BankAccount(userName, 1000);
            Console.WriteLine($"Account {account.Number} was created for {account.Owner} with {account.Balance}");

            Console.WriteLine($"Welcome, {account.Owner}.");

            account.Menu();
            var menuResponse = Console.ReadLine();
            // selects to view account balance

            if (menuResponse == "1")
            {
                Console.WriteLine($"Your account balance is: {account.Balance}");
            }
            // Selects to make a deposit
            else if (menuResponse == "2")
            {
                Console.WriteLine($"How much would you like to deposit?");
                string depositAmountInput = Console.ReadLine();
                int depositAmount = Int32.Parse(depositAmountInput);
                Console.WriteLine($"Please create a reference");
                string depositNote = Console.ReadLine();

                account.MakeDeposit(depositAmount, DateTime.Now, depositNote);
                Console.WriteLine($"Your account balance is: {account.Balance}");
            }
            // selects to make withdrawal
            else if (menuResponse == "3")
            {
                Console.WriteLine($"How much would you like to withdraw?");
                string withdrawAmountInput = Console.ReadLine();
                int withdrawAmount = Int32.Parse(withdrawAmountInput);
                Console.WriteLine($"Please create a reference");
                string withdrawNote = Console.ReadLine();

                account.MakeWithdrawal(withdrawAmount, DateTime.Now, withdrawNote);
                Console.WriteLine($"Your account balance is: {account.Balance}");
            }
            // selects to view account history
            else if (menuResponse == "4")
            {
                Console.WriteLine("Here is your account history");
                Console.WriteLine(account.GetAccountHistory());
            }
            else if (menuResponse == "5")
            {

            }


            account.EndOfService();
            Console.WriteLine(".");
            account.Menu();

            // write logic to get the if statement to run again

            // Test that you can't overdraw
            //try
            //{
            //    account.MakeWithdrawal(70000, DateTime.Now, "Tiny handbag");
            //}
            //catch (InvalidOperationException e)
            //{
            //    Console.WriteLine("Exception caught trying to overdraw");
            //    //Console.WriteLine(e.ToString());
            //}

            //// Test that the initial balances must be positive
            //try
            //{
            //    var invalidAccount = new BankAccount("invalid", -55);
            //}
            //catch (ArgumentOutOfRangeException e)
            //{
            //    Console.WriteLine("Exception caught creating account with negative balance");
            //    //Console.WriteLine(e.ToString());
            //}

            //account.MakeWithdrawal(50, DateTime.Now, "Computer Game");
            //Console.WriteLine(account.Balance);

            //Console.WriteLine(account.GetAccountHistory());


        }
    }
}