using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class AccountOverviewTable
    {
        private List<AccountOverviewItem> _item;

        private int _accountId;

        public int AccountID { get { return _accountId; } }

        public List<AccountOverviewItem> Items { get { return _item; } }

        private AccountOverviewTable() { }

        public AccountOverviewTable(Account account)
        {
            _accountId = account.ID;

            var earliestDate = account.GetEarliestDate();

            var latestDate = account.LastUpdateDate;

            var dateRange = CalculateDaysDifference(earliestDate, latestDate);

            var list = new List<AccountOverviewItem>();

            for (int i = dateRange; i >= -1; i--)
            {
                var date = earliestDate.AddDays(i);

                var datesIncomes = account.GetIncomesforDateList(date);
                var datesExpenses = account.GetExpensesforDateList(date);

                var incomesCount = 0;
                var expensesCount = 0;

                for(int j = 0; j < datesIncomes.Count && j < datesExpenses.Count; j++)
                {
                    var item = new AccountOverviewItem(account)
                    {

                        Balance = AdjustAmount(list, account.Amount),
                        Date = date,

                        IncomeAmount = datesIncomes[j].Amount,
                        IncomeType = datesIncomes[j].Type,

                        ExpenseAmount = datesExpenses[j].Amount,
                        ExpenseType = datesExpenses[j].Type
                    };

                    incomesCount++;
                    expensesCount++;

                    list.Add(item);
                }

                if (incomesCount < datesIncomes.Count)
                {
                    for (int j = incomesCount; j < datesIncomes.Count; j++)
                    {
                        var item = new AccountOverviewItem(account)
                        {
                            Balance = AdjustAmount(list, account.Amount),
                            Date = date,

                            IncomeAmount = datesIncomes[j].Amount,
                            IncomeType = datesIncomes[j].Type
                        };

                        list.Add(item);
                    }
                }
                else if (expensesCount < datesExpenses.Count)
                {
                    for (int j = expensesCount; j < datesExpenses.Count; j++)
                    {
                        var item = new AccountOverviewItem(account)
                        {
                            Balance = AdjustAmount(list, account.Amount),
                            Date = date,

                            ExpenseAmount = datesExpenses[j].Amount,
                            ExpenseType = datesExpenses[j].Type
                        };

                        list.Add(item);
                    }
                }

                if (datesExpenses.Count == 0 && datesIncomes.Count == 0)
                {
                    var item = new AccountOverviewItem(account)
                    {
                        Balance = AdjustAmount(list, account.Amount),
                        Date = date
                    };

                    list.Add(item);
                }
            }

            _item = list;
        }

        private int CalculateDaysDifference(DateTime early, DateTime late)
        {
            if (early > late) { throw new Exception("Early date is later than late date"); }

            var difference = (late.Year - early.Year) * 365;

            difference += late.DayOfYear - early.DayOfYear;

            return difference;
        }

        private double AdjustAmount(List<AccountOverviewItem> identified, double startingAmount)
        {
            if (identified.Count == 0)
            {
                return startingAmount;
            }

            var totalIncomes = 0.00;
            var totalExpenses = 0.00;

            for (int i = 0; i < identified.Count; i++)
            {
                var row = identified[i];

                if (row.IncomeAmount > 0)
                {
                    totalIncomes += row.IncomeAmount;
                }

                if (row.ExpenseAmount > 0)
                {
                    totalExpenses += row.ExpenseAmount;
                }
            }

            return startingAmount + totalExpenses - totalIncomes;
        }
    }
}
