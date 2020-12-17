using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class DataAccess
    {
        public static SQLiteConnection EstablishConnection()
        {
            var conn = new SQLiteConnection("Data Source = Data/BudgetDB.db; Version = 3;");
            return conn;
        }

        public static bool CheckDBExists()
        {
            if (!File.Exists("BudgetDB.db"))
            {
                return false;
            }

            return true;
        }

        public static void CreateDB()
        {
            SQLiteConnection.CreateFile("BudgetDB.db");
        }

        public static void CreateTables()
        {
            string Table = "CREATE TABLE IF NOT EXISTS tblProfile (ProfileID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileName TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";
           
            ExecuteNonQuery(Table);

            Table = "CREATE TABLE IF NOT EXISTS tblAccounts (AccountID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Acc_Name TEXT NOT NULL, Acc_Amount REAL NOT NULL, Acc_LastUpdateDate TEXT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);

            Table = "CREATE TABLE IF NOT EXISTS tblIncomes (IncomeID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, AccountID INTEGER NOT NULL, Inc_Amount REAL NOT NULL, Inc_Type INTEGER NOT NULL, Inc_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);

            Table = "CREATE TABLE IF NOT EXISTS tblExpenses (ExpenseID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, AccountID INTEGER NOT NULL, Exp_Amount REAL NOT NULL, Exp_Type INTEGER NOT NULL, Exp_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);

            Table = "CREATE TABLE IF NOT EXISTS tblIncomeType (IncomeTypeID INTEGER PRIMARY KEY NOT NULL, IncomeType TEXT NOT NULL)";

            ExecuteNonQuery(Table);

            Table = "CREATE TABLE IF NOT EXISTS tblExpenseType (ExpenseTypeID INTEGER PRIMARY KEY NOT NULL, ExpenseType TEXT NOT NULL)";

            ExecuteNonQuery(Table);

            Table = "CREATE TABLE IF NOT EXISTS tblDebts (DebtID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Dbt_Name TEXT NOT NULL, Dbt_Principal REAL NOT NULL, Dbt_LastUpdateDate TEXT NULL, Dbt_InterestType INTERGER NOT NULL, Dbt_NumberOfTimesApplied INTERGER NULL, Dbt_AnnualPercentageRate REAL NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);


        }

        private static void ExecuteNonQuery(string query)
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = query;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }
    }
}
