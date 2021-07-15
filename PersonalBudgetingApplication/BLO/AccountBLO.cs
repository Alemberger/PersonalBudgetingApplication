using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalBudgetingApplication.Classes;
using PersonalBudgetingApplication.Core_Objects;

namespace PersonalBudgetingApplication.BLO
{
    /// <summary>
    /// Business logic object for the Account Core Object
    /// </summary>
    class AccountBLO
    {
        public Core_Objects.Account Account { get; }

        public AccountBLO(Core_Objects.Account account)
        {
            Account = Account;
        }

        public void UpdateAccountName(string newName)
        {
            newName = newName.Trim();
            if (newName is null || newName.Length == 0) { throw new ArgumentException("Invalid name provided"); }

            foreach (Core_Objects.Account check in Account.Profile.Accounts)
            {
                if (check.AccountID == Account.AccountID) { continue; }

                if (check.Name.ToLower().Trim() == newName.ToLower()) { throw new ArgumentException("This name is already used by another Account in this profile"); }
            }

            Account.Name = newName;
        }

        public List<Transaction> GetExpenses()
        {
            var expenses = new List<Transaction>();
            if (Account.AccountID < 1 || Account.Transactions is null || Account.Transactions.Count == 0) { return expenses; }

            foreach (Transaction trans in Account.Transactions)
            {
                if (trans.Type == TransactionType.Expense)
                {
                    expenses.Add(trans);
                }
            }

            return expenses;
        }

        public List<Transaction> GetExpenses(DateTime targetDate)
        {
            var expenses = new List<Transaction>();
            if (Account.AccountID < 1 || Account.Transactions is null || Account.Transactions.Count == 0) { return expenses; }

            foreach (Transaction trans in Account.Transactions)
            {
                if (trans.Type == TransactionType.Expense)
                {
                    if (trans.Date == targetDate)
                    {
                        expenses.Add(trans);
                    }
                }
            }

            return expenses;
        }

        public List<Transaction> GetIncomes()
        {
            var incomes = new List<Transaction>();
            if (Account.AccountID < 1 || Account.Transactions is null || Account.Transactions.Count == 0) { return incomes; }

            foreach (Transaction trans in Account.Transactions)
            {
                if (trans.Type == TransactionType.Income)
                {
                    incomes.Add(trans);
                }
            }

            return incomes;
        }

        public List<Transaction> GetIncomes(DateTime targetDate)
        {
            var incomes = new List<Transaction>();
            if (Account.AccountID < 1 || Account.Transactions is null || Account.Transactions.Count == 0) { return incomes; }

            foreach (Transaction trans in Account.Transactions)
            {
                if (trans.Type == TransactionType.Income)
                {
                    if (trans.Date == targetDate)
                    {
                        incomes.Add(trans);
                    }
                }
            }

            return incomes;
        }

        public double GetExpensesForDate(DateTime targetDate)
        {
            double total = 0.00;

            foreach (Transaction operation in GetExpenses(targetDate))
            {
                total += operation.Amount.Value;
            }

            return total;
        }

        public double GetIncomesForDate(DateTime targetDate)
        {
            double total = 0.00;

            foreach (Transaction operation in GetIncomes(targetDate))
            {
                total += operation.Amount.Value;
            }

            return total;
        }
    }
}
