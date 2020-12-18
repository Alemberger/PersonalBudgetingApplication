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

        public static void ExecuteNonQuery(string query)
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
