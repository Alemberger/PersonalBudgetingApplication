using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class IncomeEntry
    {
        private int _profileId;

        private double _amount;

        private IncomeType _type;

        public int ProfileId
        {
            get
            {
                return _profileId;
            }
            set
            {
                _profileId = value;
            }
        }

        public double Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (value < 0) { value *= -1; }
                _amount = value;
            }
        }

        public IncomeType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public IncomeEntry(double amount)
        {
            Amount = amount;
        }

        public IncomeEntry(double amount, IncomeType type)
        {
            Amount = amount;
            Type = type;
        }
    }

    enum IncomeType : int
    {
        Paycheck = 1,
        Gift,
        Refund,
        Other
    }
}
