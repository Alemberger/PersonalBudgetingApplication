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

            Created = true;
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

        private void CreateTransactionsTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblTransactions (TransactionID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, AccountID INTEGER NOT NULL, Trn_Amount REAL NOT NULL, Trn_Type INTEGER NOT NULL, Trn_Category INTEGER NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateLookupsTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblLookups (LookupID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, TypeID INTEGER NOT NULL, Description TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }

        private void CreateLookupTypesTable()
        {
            var query = "CREATE TABLE IF NOT EXISTS tblLookupTypes (TypeID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, TypeName TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(query);
        }
    }
}
