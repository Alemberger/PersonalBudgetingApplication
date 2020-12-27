using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PersonalBudgetingApplication.Classes
{
    class Common
    {

        public static bool CheckAmountInput(string amount)
        {
            if (amount == "") { throw new Exception("No input provided"); }

            bool valid = false;

            //Create the regular expression to validate the input agains
            var check = new Regex("^[0-9]+\\.[0-9]{2}$");

            if (check.IsMatch(amount))
            {
                valid = true;
            }

            return valid;
        }

        public static void PopulateProfileList(ComboBox target)
        {
            //Add the first blank value to the combo box
            var Binder = new List<ComboBoxItem> { new ComboBoxItem() { Content = "" } };

            using (var conn = DataAccess.EstablishConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT ProfileName, ProfileID FROM tblProfile";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        Binder.Add(new ComboBoxItem() { Content = read.GetString(0), Tag = read.GetInt32(1).ToString() });
                    }
                    read.Close();
                    conn.Close();
                }
            }

            target.ItemsSource = Binder;  
        }

        public static void PopulateIncomeTypeList(ComboBox target)
        {
            var Binder = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT IncomeType, IncomeTypeID FROM tblIncomeType ORDER BY IncomeTypeID";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        Binder.Add(new ComboBoxItem() { Content = read.GetString(0), Tag = read.GetInt32(1).ToString() });
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            target.ItemsSource = Binder;
        }

        public static void PopulateExpenseTypeList(ComboBox target)
        {
            var Binder = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ExpenseType, ExpenseTypeID FROM tblExpenseType ORDER BY ExpenseTypeID";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        Binder.Add(new ComboBoxItem() { Content = read.GetString(0), Tag = read.GetInt32(1).ToString() });
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            target.ItemsSource = Binder;
        }

        public static int CalculateDifferenceInDays(DateTime earlyDate, DateTime lateDate)
        {
            if (earlyDate > lateDate) { throw new Exception("Early date after late date"); }

            var difference = (lateDate.Year - earlyDate.Year) * 365;

            difference += lateDate.DayOfYear - earlyDate.DayOfYear;

            return difference;
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

                    if (Main.LoadedPage == "Default")
                    {
                        Main.PrimaryFrame.Navigate(new Uri("DefaultPage.xaml", UriKind.RelativeOrAbsolute));
                    }
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
