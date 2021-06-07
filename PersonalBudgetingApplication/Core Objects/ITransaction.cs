using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Core_Objects
{
    interface ITransaction
    {
        TransactionType TransactionType { get; set; }

        double Amount { get; set; }

        void Apply(IAccount target);
    }

    public enum TransactionType
    {
        Income = 1,
        Expense
    }
}
