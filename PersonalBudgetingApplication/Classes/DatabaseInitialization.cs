using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    /// <summary>
    /// Initializes the SQLite database in case it is not already created and maintained.
    /// </summary>
    class DatabaseInitialization
    {
        /// <summary>
        /// Creates both the database file and then fills that file with tables
        /// </summary>
        public static void CreateFullDatabase()
        {
            InitializeDatabaseFile();

            CreateDatabaseTables();

            //Load type tables with enum values
            PopulateIncomeTypeTable();

            PopulateExpenseTypeTable();
        }

        /// <summary>
        /// Creates the BudgetDB.db file for the application
        /// </summary>
        public static void InitializeDatabaseFile()
        {
            SQLiteConnection.CreateFile("BudgetDB.db");
        }

        public static void CreateDatabaseTables()
        {
            CreateProfileTable();
            CreateIncomeTable();
            CreateExpenseTable();
            CreateSavingsTable();
            CreateIncomeTypeTable();
            CreateExpenseTypeTable();
        }

        private static void CreateProfileTable()
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "CREATE TABLE tblProfile (ProfileID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileName VARCHAR(50) NOT NULL);";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        private static void CreateIncomeTable()
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "CREATE TABLE tblIncome (IncomeID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Inc_Amount DOUBLE NOT NULL, Inc_Type INTEGER NOT NULL, RecordBy VARCHAR(50) NULL, RecordDate VARCHAR(50) NULL);";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        private static void CreateExpenseTable()
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "CREATE TABLE tblExpense (ExpenseID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Exp_Amount DOUBLE NOT NULL, Exp_Type INTEGER NOT NULL, RecordBy VARCHAR(50) NULL, RecordDate VARCHAR(50) NULL);";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        private static void CreateIncomeTypeTable()
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "CREATE TABLE tblIncomeType (IncomeTypeID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, IncomeType VARCHAR(50) NOT NULL);";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        private static void CreateExpenseTypeTable()
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "CREATE TABLE tblExpenseType (ExpenseTypeID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ExpenseType VARCHAR(50) NOT NULL);";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        private static void CreateSavingsTable()
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "CREATE TABLE tblSavings (SavingsID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, ProfileID INTEGER NOT NULL, Sav_Amount DOUBLE NOT NULL, RecordBy VARCHAR(50) NULL, RecordDate VARCHAR(50) NULL);";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        private static void PopulateIncomeTypeTable()
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO tblIncomeType (IncomeType) VALUES (@IncomeType)";
                    cmd.Parameters.Add("@IncomeType", DbType.String);

                    for (int i = 1; i <= (int)IncomeType.Other; i++)
                    {
                        cmd.Parameters["@IncomeType"].Value = ((IncomeType)i).ToString();

                        if (conn.State == ConnectionState.Closed) { conn.Open(); }

                        cmd.ExecuteNonQuery();

                        conn.Close();

                        cmd.Parameters["@IncomeType"].Value = null;
                    }
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        private static void PopulateExpenseTypeTable()
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO tblExpenseType (ExpenseType) VALUES (@ExpenseType)";
                    cmd.Parameters.Add("@ExpenseType", DbType.String);

                    for (int i = 1; i <= (int)ExpenseType.Other; i++)
                    {
                        cmd.Parameters["@ExpenseType"].Value = ((ExpenseType)i).ToString();

                        if (conn.State == ConnectionState.Closed) { conn.Open(); }

                        cmd.ExecuteNonQuery();

                        conn.Close();

                        cmd.Parameters["@ExpenseType"].Value = null;
                    }
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }
    }
}
