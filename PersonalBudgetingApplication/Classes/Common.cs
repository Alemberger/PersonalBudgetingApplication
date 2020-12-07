using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PersonalBudgetingApplication.Classes
{
    class Common
    {
        public static SQLiteConnection CreateConnection()
        {
            //Sets the database
            var conn = new SQLiteConnection("Data Source = C:\\PersonalBudget\\BudgetDB.db;Version=3;");
            try { conn.Open(); }
            catch (SQLiteException e)
            {
                //Confirm that the SQLiteExcpetion is caused by the lack of database file
                MessageBox.Show(e.Message);
            }
            conn.Close();
            return conn;
        }

        public static void PopulateProfileList(ComboBox target)
        {
            //Add the first blank value to the combo box
            var Binder = new List<ComboBoxItem> { new ComboBoxItem() { Content = "" } };

            using (var conn = CreateConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT ProfileName FROM tblProfile";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        Binder.Add(new ComboBoxItem() { Content = read.GetString(0) });
                    }
                    read.Close();
                    conn.Close();
                }
            }

            target.ItemsSource = Binder;
           
        }
    }
}
