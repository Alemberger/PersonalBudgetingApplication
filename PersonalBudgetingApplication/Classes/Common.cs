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
            var conn = new SQLiteConnection("Data Source = BudgetDB.db;Version=3;");
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

        public static void PopulateIncomeTypeList(ComboBox target)
        {
            var Binder = new List<ComboBoxItem> { new ComboBoxItem() { Content = "" } };

            using (var conn = CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT IncomeType FROM tblIncomeType ORDER BY IncomeTypeID";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        Binder.Add(new ComboBoxItem() { Content = read.GetString(0) });
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            target.ItemsSource = Binder;
        }

        public static void PopulateExpenseTypeList(ComboBox target)
        {
            var Binder = new List<ComboBoxItem> { new ComboBoxItem() { Content = "" } };

            using (var conn = CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ExpenseType FROM tblExpenseType ORDER BY ExpenseTypeID";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        Binder.Add(new ComboBoxItem() { Content = read.GetString(0) });
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            target.ItemsSource = Binder;
        }

        public static void ReturnToMainWindow(Profile selected)
        {
            if (selected.ProfileName == "" || selected.ProfileName is null)
            {
                try
                {
                    Application.Current.MainWindow.Show();
                    Application.Current.MainWindow.Focus();
                }
                catch (NullReferenceException)
                {
                    var Main = new MainWindow();

                    Main.Show();
                    Main.Focus();
                }
            }
            else
            {
                try
                {
                    Application.Current.MainWindow.Show();
                    Application.Current.MainWindow.Focus();

                    var Main = (MainWindow)Application.Current.MainWindow;

                    int index = 0;

                    for (int i = 0; i < Main.DDLProfileList.Items.Count; i++)
                    {
                        var Item = (ComboBoxItem)Main.DDLProfileList.Items[i];

                        if (Item.Content.ToString() == selected.ProfileName)
                        {
                            index = i;
                        }
                    }

                    Main.DDLProfileList.SelectedIndex = index;
                }
                catch (NullReferenceException)
                {
                    var Main = new MainWindow();

                    int index = 0;

                    for (int i = 0; i < Main.DDLProfileList.Items.Count; i++)
                    {
                        var Item = (ComboBoxItem)Main.DDLProfileList.Items[i];

                        if (Item.Content.ToString() == selected.ProfileName)
                        {
                            index = i;
                        }
                    }

                    Main.DDLProfileList.SelectedIndex = index;

                    Main.Show();
                    Main.Focus();
                }
            }
        }
    }
}
