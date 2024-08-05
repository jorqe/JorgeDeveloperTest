using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using ClosedXML.Excel;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using JorgeDeveloperTest.Models;
using Newtonsoft.Json;
using System.Data;


namespace JorgeDeveloperTest.CRUD
{
    static public class TransactionsManager
    {
        //THE PATH TO JSON ON MY LOCAL MACHINE
        private static string pathToJson = "D:\\DUsers\\DJorge\\source\\repos\\JorgeDeveloperTest\\JorgeDeveloperTest\\Resources\\Data.json";
        private static string excelFilePath = "D:\\DUsers\\DJorge\\source\\repos\\JorgeDeveloperTest\\JorgeDeveloperTest\\Resources\\Data.xlsx";

        static public void DisplayAccounts(List<Accounts> accounts, List<Transactions> transactions)
        {


            Console.Clear();
            Console.WriteLine("Developer Test: Account and Transaction Management Application");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("         Current Accounts");
            Console.WriteLine("");

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Developer Test: Account and Transaction Management Application");
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("         Current Accounts");
                Console.WriteLine("");


                foreach (Accounts account in accounts)
                    Console.WriteLine($"Account Info: {account.name} (Id: {account.id})");
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Select by Id which account you would like to view\nType '0' to exit\nType '4' To Export Current State Of Json To excel ");

                Console.Write("Enter Your Choice: ");
                string selectedID = Console.ReadLine();

                if (selectedID == "4")
                {
                   
                    ExportToExcel(accounts, transactions);
                    Console.WriteLine("Data exported to Excel successfully.");
                    return;
                }
                else if (int.TryParse(selectedID, out int numselectedID))
                {
                    if (numselectedID == 0)
                    {
                        return;
                    }
                    var selectedAccount = accounts.Find(a => a.id == numselectedID);
                    if (selectedAccount != null)
                    {
                        DisplayTransactions(selectedAccount, transactions);
                        CrudMenu(selectedAccount, transactions, accounts);
                    }
                    else
                    {
                        Console.WriteLine("Not a Valid Account ID. Please Enter a valid account id");
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
            Console.WriteLine("1. Create a new transaction");
            Console.WriteLine("2. Modify an existing transaction");
            Console.WriteLine("3. Delete a transaction");
            Console.WriteLine("0. Return to account selection");

            string chosenOption = Console.ReadLine();
           
            switch(chosenOption)
            {
                case "1":
                    CreateTransaction(seletctedAccount, transactions, accounts);
                    break;
                case "2":
                    UpdateTransaction(seletctedAccount, transactions, accounts);
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
            Console.Clear();
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Create A New Transaction\n");
            
            Console.Write("Enter Transaction description: ");
            string description = Console.ReadLine();

            Console.Write("Enter Transaction Type debit or credit: ");
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
        
        public static void UpdateTransaction(Accounts selectedAccount, List<Transactions> transactions, List<Accounts> accounts)
        {
            //Console.WriteLine("UPDATINGGGG");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Modify an Exisiting Transaction\n");
            Console.Write("Select Transaction  Id for the transaction you would like to modfiy: ");
            string inputId = Console.ReadLine();
            if (int.TryParse(inputId, out int transactionId))
            {
                //searching list using Firstordefault for first match as transactonsid selected
                Transactions transactionToUpdate = transactions.FirstOrDefault(t => t.id == transactionId);

                //ensuring transaction exist
                if (transactionToUpdate != null)
                {
                    //decided to display current trans info for easier use to user in console
                    Console.WriteLine($"Current Description: {transactionToUpdate.description}");
                    Console.WriteLine($"Current Debit or Credit: {transactionToUpdate.debit_credit}");
                    Console.WriteLine($"Current Amount: {transactionToUpdate.amount}");
                    Console.WriteLine("---------------------------------------------------------------");

                    
                    Console.Write("Enter new description: ");
                    transactionToUpdate.description = Console.ReadLine();
                    
                    
                    Console.Write("Enter new Debit or Credit: ");
                    string newDebitCredit = Console.ReadLine().ToLower();
                    if (newDebitCredit == "debit" || newDebitCredit == "credit")
                    {
                        if (transactionToUpdate.debit_credit == "debit")
                        {
                            selectedAccount.current_balance += transactionToUpdate.amount;
                        }
                        else if (transactionToUpdate.debit_credit == "credit")
                        {
                            selectedAccount.current_balance -= transactionToUpdate.amount;
                        }
                        transactionToUpdate.debit_credit = newDebitCredit;

                        if (newDebitCredit == "debit")
                        {
                            selectedAccount.current_balance -= transactionToUpdate.amount;
                        }
                        else if (newDebitCredit == "credit")
                        {
                            selectedAccount.current_balance += transactionToUpdate.amount;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Debit or Credit input");
                    }

                    Console.Write("Enter new amount: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newAmount))
                    {
                        if (transactionToUpdate.debit_credit == "debit")
                        {
                            selectedAccount.current_balance += transactionToUpdate.amount - newAmount;
                        }
                        else if (transactionToUpdate.debit_credit == "credit")
                        {
                            selectedAccount.current_balance += newAmount - transactionToUpdate.amount;
                        }

                        transactionToUpdate.amount = newAmount;
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount.");
                    }

                    SaveData(accounts, transactions);
                    Console.WriteLine("Transaction successfully updated.");
                }
                else
                {
                    Console.WriteLine("Transaction not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Transaction ID. Please enter a valid number.");
            }


        }
        public static void DeleteTransaction(Accounts selectedAccount, List<Transactions> transactions, List<Accounts>accounts)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Delete A Transaction\n");
            Console.Write("Select Transaction Id for the transaction you would like to delete: ");
            string inputId = Console.ReadLine();
            if (int.TryParse(inputId, out int transactionId))
            {
                //finding first match
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

                    //method for writing and saving json
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
            File.WriteAllText(pathToJson, json);
        }
        public static void ExportToExcel(List<Accounts> accounts, List<Transactions> transactions)
        {
            

            using (var workbook = new XLWorkbook())
            {
                var accountsWorksheet = workbook.Worksheets.Add("Accounts");
                accountsWorksheet.Cell(1, 1).Value = "ID";
                accountsWorksheet.Cell(1, 2).Value = "Name";
                accountsWorksheet.Cell(1, 3).Value = "Number";
                accountsWorksheet.Cell(1, 4).Value = "Current Balance";

                int row = 2;
                foreach (var account in accounts)
                {
                    accountsWorksheet.Cell(row, 1).Value = account.id;
                    accountsWorksheet.Cell(row, 2).Value = account.name;
                    accountsWorksheet.Cell(row, 3).Value = account.number;
                    accountsWorksheet.Cell(row, 4).Value = account.current_balance;
                    row++;
                }

                var transactionsWorksheet = workbook.Worksheets.Add("Transactions");
                transactionsWorksheet.Cell(1, 1).Value = "ID";
                transactionsWorksheet.Cell(1, 2).Value = "Description";
                transactionsWorksheet.Cell(1, 3).Value = "Debit/Credit";
                transactionsWorksheet.Cell(1, 4).Value = "Amount";
                transactionsWorksheet.Cell(1, 5).Value = "Account ID";

                int row2 = 2;
                foreach (var transaction in transactions)
                {
                    transactionsWorksheet.Cell(row2, 1).Value = transaction.id;
                    transactionsWorksheet.Cell(row2, 2).Value = transaction.description;
                    transactionsWorksheet.Cell(row2, 3).Value = transaction.debit_credit;
                    transactionsWorksheet.Cell(row2, 4).Value = transaction.amount;
                    transactionsWorksheet.Cell(row2, 5).Value = transaction.account_id;
                    row2++;
                }

                workbook.SaveAs(excelFilePath);
            }
        }

    }
}
