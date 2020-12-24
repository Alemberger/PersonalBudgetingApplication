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

        public string DateString
        {
            get
            {
                return Date.ToString("MM/dd/yyyy");
            }
        }

        public double Balance { get; set; }

        public double IncomeAmount { get; set; }

        public IncomeType IncomeType { get; set; }

        public string IncomeTypeName
        {
            get
            {
                var output = IncomeType.ToString();

                if (output == "0") { return ""; }
                else { return output; }
            }
        }

        public double ExpenseAmount { get; set; }

        public ExpenseType ExpenseType { get; set; }

        public string ExpenseTypeName
        {
            get
            {
                var output = ExpenseType.ToString();

                if (output == "0") { return ""; }
                else { return output; }
            }
        }

        private AccountOverviewItem() { }

        public AccountOverviewItem(Account account)
        {
            AccountID = account.ID;
        }
    }
}
