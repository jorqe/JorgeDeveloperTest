using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JorgeDeveloperTest.Models;

namespace JorgeDeveloperTest
{
    //maping json data to our C# objects
    public class rootobject
    {
        public List<Accounts> accounts { get; set; }
        public List<Transactions> transactions { get; set; }

    }
}
