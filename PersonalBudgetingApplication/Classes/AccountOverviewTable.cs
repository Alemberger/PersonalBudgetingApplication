using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class AccountOverviewTable
    {
        public List<AccountOverviewItem> Items { get; set; }

        private AccountOverviewTable() { }

        public AccountOverviewTable(Account account)
        {
            var incomes = account.Incomes;

            var expenses = account.Expenses;

            var earliestDate = account.LastUpdateDate;

            var latestDate = account.LastUpdateDate;

            for (int i = 0; i < incomes.Count; i++)
            {
                if (incomes[i].Date < earliestDate)
                {
                    earliestDate = incomes[i].Date;
                }

                if (incomes[i].Date > latestDate)
                {
                    latestDate = incomes[i].Date;
                }
            }

            for (int i = 0; i < expenses.Count; i++)
            {
                if (expenses[i].Date < earliestDate)
                {
                    earliestDate = expenses[i].Date;
                }

                if (expenses[i].Date > latestDate)
                {
                    latestDate = expenses[i].Date;
                }
            }

            var dateRange = CalculateDaysDifference(earliestDate, latestDate);

            var list = new List<AccountOverviewItem>();

            for (int i = dateRange; i > 0; i--)
            {
                var date = latestDate.AddDays(-i);

                int iCount = 0;
                int eCount = 0;

                for (int j = 0; j < incomes.Count; j++)
                {
                    if (incomes[j].Date.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd"))
                    {
                        iCount++;
                    }
                }

                for (int j = 0; j < expenses.Count; j++)
                {
                    if (expenses[j].Date.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd"))
                    {
                        eCount++;
                    }
                }

                Income[] datesIncomes = new Income[iCount];
                Expense[] datesExpenses = new Expense[eCount];

                for (int j = 0; j < iCount; j++)
                {
                    for (int y = 0; y < incomes.Count; y++)
                    {
                        if (incomes[y].Date.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd"))
                        {
                            var present = false;
                            foreach (Income found in datesIncomes)
                            {
                                if (found.ID == incomes[y].ID)
                                {
                                    present = true;
                                    break;
                                }
                            }

                            if (present) { continue; }
                            datesIncomes[j] = incomes[y];
                            break;
                        }
                    }
                }

                for (int j = 0; j < eCount; j++)
                {
                    for (int y = 0; y < expenses.Count; y++)
                    {
                        if (expenses[y].Date.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd"))
                        {
                            var present = false;
                            foreach (Expense found in datesExpenses)
                            {
                                if (found.ID == expenses[y].ID)
                                {
                                    present = true;
                                    break;
                                }
                            }

                            if (present) { continue; }
                            datesExpenses[j] = expenses[y];
                            break;
                        }
                    }
                }

                for (int j = 0; j < iCount && j < eCount; j++)
                {
                    var item = new AccountOverviewItem(account);

                    item.Date = date;

                    item.Balance = AdjustAmount(list, account.Amount);

                    item.IncomeAmount = datesIncomes[j].Amount;
                    item.IncomeType = datesIncomes[j].Type;

                    item.ExpenseAmount = datesExpenses[j].Amount;
                    item.ExpenseType = datesExpenses[j].Type;

                    list.Add(item);
                }

                if (iCount > eCount)
                {
                    for (int j = eCount; j < iCount; j++)
                    {
                        var item = new AccountOverviewItem(account);

                        item.Date = date;

                        item.Balance = AdjustAmount(list, account.Amount);

                        item.IncomeAmount = datesIncomes[j].Amount;
                        item.IncomeType = datesIncomes[j].Type;

                        list.Add(item);
                    }
                }
                else if (iCount < eCount)
                {
                    for (int j = iCount; j < eCount; j++)
                    {
                        var item = new AccountOverviewItem(account);

                        item.Date = date;
                        item.Balance = AdjustAmount(list, account.Amount);

                        item.ExpenseAmount = datesExpenses[j].Amount;
                        item.ExpenseType = datesExpenses[j].Type;

                        list.Add(item);
                    }
                }
            }

            Items = list;
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
