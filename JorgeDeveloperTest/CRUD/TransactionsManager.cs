using System;
using System.Collections.Generic;
using System.Linq;
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

            Console.WriteLine("Developer Test: Account and Transaction Management Application");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("         Current Accounts");
            Console.WriteLine("");
            foreach (Accounts account in accounts)
                Console.WriteLine($"Account Info:{account.name} (Id:{account.id})");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Selected by Id Which account you would like to view");
            Console.WriteLine("or Press 0 To exit");

            string selectedID = Console.ReadLine();
            int numselectedID = 0;
            Console.WriteLine("---------------------------------------------------------------");
            if (int.TryParse(selectedID,out numselectedID))
            {
                if (numselectedID == 1)

                    foreach (Transactions t in transactions)
                    {
                        
                        if (t.account_id == 1)
                        {
                            
                            Console.WriteLine($"Transaction Id {t.id}");
                            Console.WriteLine($"Description: {t.description} ");
                            Console.WriteLine($"Debit or Credit: {t.debit_credit} ");
                            Console.WriteLine($"Amount: {t.amount} ");
                            Console.WriteLine("");
                            
                        }
                    }
                else if (numselectedID == 2)
                {
                    foreach (Transactions t in transactions)
                    {
                        if (t.account_id == 2)
                        {
                            Console.WriteLine($"Transaction Id {t.id}");
                            Console.WriteLine($"Description: {t.description} ");
                            Console.WriteLine($"Debit or Credit: {t.debit_credit} ");
                            Console.WriteLine($"Amount: {t.amount} ");
                            Console.WriteLine("");
                        }
                    }
                }
                else if (numselectedID == 0)
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("NONOPLEASE NO");

                }
                     

            }
        
            else
            {
                Console.WriteLine("NONONONO");
            }
            Console.WriteLine("---------------------------------------------------------------");
            Console.ReadKey();

            /**
            foreach (Accounts i in accounts)
                Console.WriteLine(i.name);

            foreach (Transactions t in transactions)
                Console.WriteLine(t.description);

            Console.ReadKey();
            **/
        }


    }
}
