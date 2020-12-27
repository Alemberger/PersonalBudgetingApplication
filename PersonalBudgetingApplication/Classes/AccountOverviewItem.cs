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

        public string IncomeDisplay
        {
            get
            {
                string display;

                try
                {
                    display = string.Format("{0:C}", IncomeAmount);
                }
                catch (FormatException) { return ""; }

                if (display == "$0.00") { return ""; }

                return display;
            }
        }

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

        public string ExpenseDisplay
        {
            get
            {
                string display;

                try
                {
                    display = string.Format("{0:C}", ExpenseAmount);
                }
                catch (FormatException) { return ""; }

                if (display == "$0.00") { return ""; }

                return display;
            }
        }

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
