using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class SavingsEntry
    {
        private double _amount;

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public SavingsEntry(double amount)
        {
            Amount = amount;
        }
    }
}
