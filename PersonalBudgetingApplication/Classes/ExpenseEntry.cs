using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class ExpenseEntry
    {
        private double _amount;

        public int ProfileId { get; set; }

        public int ExpenseId { get; set; }

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

        public ExpenseType Type { get; set; }

        public string Date { get; set; }

        public ExpenseEntry() { }

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

    public enum ExpenseType : int
    {
        Rent = 1,
        Utilities,
        Food,
        Entertainment,
        Vehicle,
        Other
    }
}
