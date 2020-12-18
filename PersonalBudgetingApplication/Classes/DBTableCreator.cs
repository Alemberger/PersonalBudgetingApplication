using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class DBTableCreator : DataAccess
    {
        public bool Created { get; set; }
        public DBTableCreator() 
        {
            Created = false;
        }

        public DBTableCreator(bool createTables)
        {
            if (createTables)
            {
                CreateTables();
            }

            Created = createTables;
        }

        public void CreateTables()
        {
            CreateProfileTable();

            CreateAccountsTable();

            CreateIncomesTable();

            CreateExpensesTable();

            CreateDebtsTable();

            CreatePaymentsTable();

            CreateInterestTable();
        }

        private void CreateProfileTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblProfile (ProfileID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileName TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateAccountsTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblAccounts (AccountID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Acc_Name TEXT NOT NULL, Acc_Amount REAL NOT NULL, Acc_LastUpdateDate TEXT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateIncomesTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblIncomes (IncomeID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, AccountID INTEGER NOT NULL, Inc_Amount REAL NOT NULL, Inc_Type INTEGER NOT NULL, Inc_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateExpensesTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblExpenses (ExpenseID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, AccountID INTEGER NOT NULL, Exp_Amount REAL NOT NULL, Exp_Type INTEGER NOT NULL, Exp_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateDebtsTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblDebts (DebtID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Dbt_Name TEXT NOT NULL, Dbt_Principal REAL NOT NULL, Dbt_LastUpdateDate TEXT NULL, Dbt_InterestType INTERGER NOT NULL, Dbt_NumberOfTimesApplied INTERGER NULL, Dbt_AnnualPercentageRate REAL NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreatePaymentsTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblDebtPayments (PaymentID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, DebtID INTEGER NOT NULL, Pmt_Amount REAL NOT NULL, Pmt_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateInterestTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblDebtInterests (InterestID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, DebtID INTEGER NOT NULL, Int_Amount REAL NOT NULL, Int_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }
    }
}
