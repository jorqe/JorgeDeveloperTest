using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using JorgeDeveloperTest.Models;
using Newtonsoft.Json;

namespace JorgeDeveloperTest.CRUD
{
    static public class TransactionsManager
    {

        static public void DisplayAccounts(List<Accounts> accounts, List<Transactions> transactions)
        {


            Console.Clear();
            Console.WriteLine("Developer Test: Account and Transaction Management Application");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("         Current Accounts");
            Console.WriteLine("");
            foreach (Accounts account in accounts)
                Console.WriteLine($"Account Info: {account.name} (Id: {account.id})");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Select by Id which account you would like to view or Press 0 to exit");
            while (true)
            {


                string selectedID = Console.ReadLine();

                if (int.TryParse(selectedID, out int numselectedID))
                {
                    if (numselectedID == 0)
                    {
                        break;
                    }
                    var selectedAccount = accounts.Find(a => a.id == numselectedID);
                    if (selectedAccount != null)
                    {
                        DisplayTransactions(selectedAccount, transactions);

                        CrudMenu(selectedAccount, transactions);

                    }
                    else
                    {
                        Console.WriteLine("NOt a Valid Account ID. Please Enter a valid account id");

                    }
                }

                else
                {
                    Console.WriteLine("Please Enter a Number.");
                }


                /**
                foreach (Accounts i in accounts)
                    Console.WriteLine(i.name);

                foreach (Transactions t in transactions)
                    Console.WriteLine(t.description);

                Console.ReadKey();
                **/

            }
        }
        private static void DisplayTransactions(Accounts account, List<Transactions> transactions)
        {
            //Clearing the console
            Console.Clear();
            Console.WriteLine($"Account Information: {account.name} (ID: {account.id})");
            Console.WriteLine("---------------------------------------------------------------");

            var accountTransactions = transactions.FindAll(t => t.account_id == account.id);
            foreach (var t in accountTransactions)
            {
                Console.WriteLine($"Transaction Id: {t.id}");
                Console.WriteLine($"Description: {t.description}");
                Console.WriteLine($"Debit or Credit: {t.debit_credit}");
                Console.WriteLine($"Amount: {t.amount}");
                Console.WriteLine("");
            }
        }

        //Create new method for crud (Create, read, update, delete) operations for transactions 
        // all except read since we do that in displayTransactions
        public static void CrudMenu(Accounts seletctedAccount, List<Transactions> transactions)
        {
            Console.Write("Select Option: \n");
            Console.WriteLine("1. Add a new transaction for an account");
            Console.WriteLine("2. Modify an existing transaction");
            Console.WriteLine("3. Remove a transaction");
            Console.WriteLine("0. Return to account selection");

            string chosenOption = Console.ReadLine();
           
            switch(chosenOption)
            {
                case "1":
                    CreateTransaction(seletctedAccount, transactions);
                    break;
                case "2":
                    UpdateTransaction(seletctedAccount, transactions);
                    break;
                case "3":
                    DeleteTransaction(seletctedAccount, transactions);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Please input a valid option.");
                    break;

            }

        }
        public static void CreateTransaction(Accounts selectedAccount,  List<Transactions> transactions)
        {
            Console.WriteLine("CREATINGGGGGGG");
        }
        public static void UpdateTransaction(Accounts selectedAccount, List<Transactions> transactions)
        {
            Console.WriteLine("UPDATINGGGG");

        }
        public static void DeleteTransaction(Accounts selectedAccount, List<Transactions> transactions)
        {
            Console.WriteLine("DELETTIONGG");
        }
    }
}
