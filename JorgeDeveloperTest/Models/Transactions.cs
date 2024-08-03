using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JorgeDeveloperTest.Models
{
    public class Transactions
    {
        public int id { get; set; }
        public string description { get; set; }
        public string debit_credit { get; set; }
        public decimal amount { get; set; }
        public string account_id { get; set; }
    }
}
