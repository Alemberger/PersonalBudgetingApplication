using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class SavingsEntry
    {

        public int ProfileId { get; set; }

        public double Amount { get; set; }

        public SavingsEntry() { }

        public SavingsEntry(double amount)
        {
            Amount = amount;
        }
    }
}
