using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JorgeDeveloperTest.CRUD;
using JorgeDeveloperTest.Models;
using Newtonsoft.Json;

//using JorgeDeveloperTest.Models;

namespace JorgeDeveloperTest
{
    public class program
    {
        static void Main(string[] args)
        {
            //Creating the strucutres(list) to hold information we are going to parse from json. 
            List<Accounts> accounts = new List<Accounts>();
            List<Transactions> transactions = new List<Transactions>();

            //reading json file 
            string jsonText = File.ReadAllText("D:\\DUsers\\DJorge\\source\\repos\\JorgeDeveloperTest\\JorgeDeveloperTest\\Resources\\Data.json");
            var data = JsonConvert.DeserializeObject<rootobject>(jsonText);

            //retrieving acc/transa from root.
            accounts = data.accounts;
            transactions = data.transactions;
            //data.accounts and data.transactions were empty list that now hold information pulled from json

            TransactionsManager.DisplayAccounts(accounts, transactions);





            /**
            foreach(Accounts i in accounts) 
                Console.WriteLine(i.name);

            foreach(Transactions t in transactions) 
                    Console.WriteLine(t.description);

            Console.ReadKey();
            **/

        }

    }
}
