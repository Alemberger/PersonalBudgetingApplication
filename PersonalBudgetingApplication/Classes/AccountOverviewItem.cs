using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class AccountOverviewItem
    {
        public int AccountID { get; set; }

        public DateTime Date { get; set; }

        public double Balance { get; set; }

        public double IncomeAmount { get; set; }

        public IncomeType IncomeType { get; set; }

        public double ExpenseAmount { get; set; }

        public ExpenseType ExpenseType { get; set; }

        private AccountOverviewItem() { }

        public AccountOverviewItem(Account account)
        {
            AccountID = account.ID;
        }
    }
}
