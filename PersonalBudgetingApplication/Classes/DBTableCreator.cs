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
            Created = !createTables;

            if (createTables)
            {
                CreateTables();
            }
        }

        public void CreateTables()
        {
            CreateProfileTable();

            CreateAccountsTable();

            CreateTransactionsTable();

            CreateLookupsTable();

            CreateLookupTypesTable();

            InitializeLookupTypesTable();

            InitializeLookupsTable();

            Created = true;
        }

        private void CreateProfileTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblProfile (ProfileID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileName TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateAccountsTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblAccount (AccountID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Acc_Name TEXT NOT NULL, Acc_Amount REAL NOT NULL, Acc_Type INT NOT NULL, Acc_LastUpdateDate TEXT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateTransactionsTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblTransaction (TransactionID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, AccountID INTEGER NOT NULL, Trn_Amount REAL NOT NULL, Trn_date TEXT NOT NULL, Trn_Type INTEGER NOT NULL, Trn_Category INTEGER NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateLookupsTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblLookup (LookupID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, TypeID INTEGER NOT NULL, Description TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateLookupTypesTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblLookupType (TypeID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, TypeName TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void InitializeLookupTypesTable()
        {
            var query = string.Format("INSERT INTO tblLookupType (TypeName, RecordBy, RecordDate) VALUES ('AccountType','init','{0}'), ('ExpenseCategory', 'init', '{0}'), ('IncomeCategory', 'init', '{0}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm")); ;

            ExecuteNonQuery(query);
        }

        private void InitializeLookupsTable()
        {
            var query = string.Format("INSERT INTO tblLookup (TypeID, Description, RecordBy, RecordDate) VALUES (1,'Banking', 'init','{0}'), (1, 'CreditCard', 'init', '{0}'), (1, 'Debt', 'init', '{0}'), (2, 'Food', 'init', '{0}'), (2, 'Rent', 'init', '{0}'), (2, 'OtherBills', 'init', '{0}'), (2, ('Other', 'init', '{0}'), (3, 'Paycheck', 'init', '{0}'), (3, 'Gift', 'init', '{0}'), (3, 'Other', 'init', '{0}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            ExecuteNonQuery(query);
        }
    }
}
