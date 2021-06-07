using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Core_Objects
{
    interface IAccount
    {
        AccountType AccountType { get; }

        double CurrentBalance { get; set; }

        void AddBalance(double amount);

        void SubtractBalance(double amount);
    }

    public enum AccountType
    {
        Default = 1,
        Credit,
    }
}
