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
    /// <summary>
    /// Provides methods for creating and accessing the data in the BudgetDB.db SQLite database
    /// </summary>
    class DataAccess
    {
        public static SQLiteConnection EstablishConnection()
        {
            var conn = new SQLiteConnection("Data Source = Data/BudgetDB.db; Version = 3;");
            return conn;
        }

        public static bool CheckDBExists()
        {
            if (!Directory.Exists("Data"))
            {
                return false;
            }

            if (!File.Exists("BudgetDB.db"))
            {
                return false;
            }

            return true;
        }

        public static void CreateDB()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            
            SQLiteConnection.CreateFile("Data/BudgetDB.db");
        }

        public static void CreateTables()
        {
            //Create the profile Table
            string Table = "CREATE TABLE IF NOT EXISTS tblProfile (ProfileID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileName TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";
           
            ExecuteNonQuery(Table);

            //Cretae the Accounts Table
            Table = "CREATE TABLE IF NOT EXISTS tblAccounts (AccountID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Acc_Name TEXT NOT NULL, Acc_Amount REAL NOT NULL, Acc_LastUpdateDate TEXT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);

            //Create the incomes table
            Table = "CREATE TABLE IF NOT EXISTS tblIncomes (IncomeID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, AccountID INTEGER NOT NULL, Inc_Amount REAL NOT NULL, Inc_Type INTEGER NOT NULL, Inc_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);

            //Create the expenses table
            Table = "CREATE TABLE IF NOT EXISTS tblExpenses (ExpenseID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, AccountID INTEGER NOT NULL, Exp_Amount REAL NOT NULL, Exp_Type INTEGER NOT NULL, Exp_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);

            //Create the Income Types table
            Table = "CREATE TABLE IF NOT EXISTS tblIncomeType (IncomeTypeID INTEGER PRIMARY KEY NOT NULL, IncomeType TEXT NOT NULL)";

            ExecuteNonQuery(Table);

            //Create the expense types table
            Table = "CREATE TABLE IF NOT EXISTS tblExpenseType (ExpenseTypeID INTEGER PRIMARY KEY NOT NULL, ExpenseType TEXT NOT NULL)";

            ExecuteNonQuery(Table);

            //Create the debts table
            Table = "CREATE TABLE IF NOT EXISTS tblDebts (DebtID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Dbt_Name TEXT NOT NULL, Dbt_Principal REAL NOT NULL, Dbt_LastUpdateDate TEXT NULL, Dbt_InterestType INTERGER NOT NULL, Dbt_NumberOfTimesApplied INTERGER NULL, Dbt_AnnualPercentageRate REAL NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);

            //Create the debt payments table
            Table = "CREATE TABLE IF NOT EXISTS tblDebtPayments (PaymentID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, DebtID INTEGER NOT NULL, Pmt_Amount REAL NOT NULL, Pmt_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);

            //Create the debt interest applications table
            Table = "CREATE TABLE IF NOT EXISTS tblDebtInterests (InterestID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, DebtID INTEGER NOT NULL, Int_Amount REAL NOT NULL, Int_Date TEXT NOT NULL, RecordBy TEXT NULL, RecordDate TEXT NULL)";

            ExecuteNonQuery(Table);
        }

        private static void ExecuteNonQuery(string query)
        {
            using (var conn = EstablishConnection())
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
