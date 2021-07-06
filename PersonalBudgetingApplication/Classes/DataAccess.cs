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

            if (!File.Exists("Data/BudgetDB.db"))
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

        public static DataTable ExecuteSelectQuery(string query, List<DataColumn> columns)
        {
            DataTable results = new DataTable();

            foreach (DataColumn column in columns)
            {
                results.Columns.Add(column); 
            }

            using (var conn = EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = query;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    if (read.FieldCount != columns.Count) { throw new DataException("Incorrect number of columns"); }

                    while (read.Read())
                    {
                        DataRow row = results.NewRow();
                        for(int i = 0; i < columns.Count; i++)
                        {
                            row[i] = ConvertValue(read[i], results.Columns[i].DataType);
                        }
                        results.Rows.Add(row);
                    }
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return results;
        }

        private static object ConvertValue(object value, Type target)
        {
            if (value.GetType() == typeof(DBNull))
            {
                if (target == typeof(string)) { return ""; }
                else { return null; }
            }

            if (target == Type.GetType("System.String"))
            {
                return value.ToString();
            }
            else if (target == Type.GetType("System.Int32"))
            {
                return Convert.ToInt32(value);
            }
            else if (target == Type.GetType("System.DateTime"))
            {
                return DateTime.Parse(value.ToString());
            }
            else if (target == Type.GetType("System.Double"))
            {
                return Convert.ToDouble(value);
            }
            else
            {
                throw new ArgumentException("Invalid Target Type provided");
            }
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
