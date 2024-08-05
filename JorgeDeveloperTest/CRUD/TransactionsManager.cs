using System;
using System.IO;
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
            Console.WriteLine("Select by Id which account you would like to view\nor Enter 0 to exit");
            while (true)
            {

                Console.Write("Enter Account ID: ");
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

                        CrudMenu(selectedAccount, transactions, accounts);

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
            Console.Clear();
            Console.WriteLine($"Account Name: {account.name} (ID: {account.id})");
            Console.WriteLine($"Account Number: {account.number}");
            Console.WriteLine($"Account Current Balance: {account.current_balance}");
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
        public static void CrudMenu(Accounts seletctedAccount, List<Transactions> transactions, List<Accounts> accounts)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.Write("Select Option: \n");
            Console.WriteLine("1. Add a new transaction for an account");
            Console.WriteLine("2. Modify an existing transaction");
            Console.WriteLine("3. Remove a transaction");
            Console.WriteLine("0. Return to account selection");

            string chosenOption = Console.ReadLine();
           
            switch(chosenOption)
            {
                case "1":
                    CreateTransaction(seletctedAccount, transactions, accounts);
                    break;
                case "2":
                    UpdateTransaction(seletctedAccount, transactions);
                    break;
                case "3":
                    DeleteTransaction(seletctedAccount, transactions, accounts);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Please input a valid option.");
                    break;

            }

        }
        public static void CreateTransaction(Accounts selectedAccount,  List<Transactions> transactions, List<Accounts> accounts)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Create A New Transaction\n");
            
            Console.Write("Enter Transaction description: ");
            string description = Console.ReadLine();

            Console.Write("Enter Transaction Type debit/credit: ");
            string debitCredit = Console.ReadLine().ToLower();

            //
            Console.Write("Enter Transaction amount: ");
            decimal transactionAmount;
            while (!decimal.TryParse(Console.ReadLine(), out transactionAmount))
            {
                Console.Write("Invalid input.\nPlease enter a valid decimal number for the amount:");
            }
            //Almost able to create new transaction
            //We need to create a unique id so we will check the list and go from there.

            //transaction id's are unique even through multiple accounts. 
            //Console.WriteLine(transactions.Max(t => t.id));
            int newTransactionId = transactions.Max(t => t.id) + 1;

            //now that we have all info we will instantiate a new transactioon object.....
            Transactions newTransaction = new Transactions
            {
                id = newTransactionId,
                description = description,
                debit_credit = debitCredit,
                amount = transactionAmount,
                account_id = selectedAccount.id,

            };
            //this will not change json but the list we have in memory that was created in "program.cs"
            transactions.Add(newTransaction);

            /** -----Testing to see if transaction was added to list-----------
            var accountTransactions = transactions.FindAll(t => t.account_id == selectedAccount.id); 
            foreach (var t in accountTransactions)
            {
                Console.WriteLine($"Transaction Id: {t.id}");
                Console.WriteLine($"Description: {t.description}");
                Console.WriteLine($"Debit or Credit: {t.debit_credit}");
                Console.WriteLine($"Amount: {t.amount}");
                Console.WriteLine("");
            }**/
            if (debitCredit == "debit")
            {
                selectedAccount.current_balance -= transactionAmount;
            }
            else if (debitCredit == "credit")
            {
                selectedAccount.current_balance += transactionAmount;
            }
            else
            {
                Console.WriteLine("Invalid transaction type. Please enter 'debit' or 'credit'.");
                return;
            }
            SaveData(accounts, transactions);
            Console.WriteLine("Transaction created and saved.");


        }
        
        public static void UpdateTransaction(Accounts selectedAccount, List<Transactions> transactions)
        {
            Console.WriteLine("UPDATINGGGG");

        }
        public static void DeleteTransaction(Accounts selectedAccount, List<Transactions> transactions, List<Accounts>accounts)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Delete A Transaction\n");
            Console.Write("Select Transaction Id for the transaction you would like to delete: ");
            string inputId = Console.ReadLine();
            if (int.TryParse(inputId, out int transactionId))
            {
                var transactionToDelete = transactions.FirstOrDefault(t => t.id == transactionId && t.account_id == selectedAccount.id);


                //making sure we update current balance of account after removing a transaction woth its corresponding amounts
                if (transactionToDelete != null)
                {
              
                    if (transactionToDelete.debit_credit.ToLower() == "debit")
                    {
                        selectedAccount.current_balance += transactionToDelete.amount;
                    }
                    else if (transactionToDelete.debit_credit.ToLower() == "credit")
                    {
                        selectedAccount.current_balance -= transactionToDelete.amount;
                    }

                    transactions.Remove(transactionToDelete);

                 
                    SaveData(accounts, transactions);

                    Console.WriteLine("Transaction deleted.");
                }
                else
                {
                    Console.WriteLine("Transaction not found or does not belong to the selected account.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Transaction ID. Please enter a valid number.");
            }

        }
        public static void SaveData(List<Accounts> accounts, List<Transactions> transactions)
        {
            var data = new { accounts = accounts, transactions = transactions };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText("D:\\DUsers\\DJorge\\source\\repos\\JorgeDeveloperTest\\JorgeDeveloperTest\\Resources\\Data.json", json);
        }
    }
}
