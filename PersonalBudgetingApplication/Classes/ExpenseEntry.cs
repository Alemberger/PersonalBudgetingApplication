using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class ExpenseEntry
    {
        private double _amount;

        private ExpenseType _type;

        public double Amount
        {
            get {
                return _amount;
            }
            set
            {
                //Converts negative amounts to positve numbers
                if (value < 0) { value *= -1; }
                _amount = value;
            }
        }

        public ExpenseType Type { get { return _type; } set { _type = value; } }

        public ExpenseEntry(double amount)
        {
            Amount = amount;
        }

        public ExpenseEntry(double amount, ExpenseType type)
        {
            
            Amount = amount;
            Type = type;
        }
    }

    enum ExpenseType : int
    {
        Rent = 1,
        Utilities,
        Food,
        Entertainment,
        Vehicle,
        Other
    }
}
