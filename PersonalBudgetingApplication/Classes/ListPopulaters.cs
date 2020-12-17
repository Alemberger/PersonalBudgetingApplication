using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PersonalBudgetingApplication.Classes
{
    class ListPopulaters : DataAccess
    {
        public void PopulateProfileList(ComboBox target)
        {
            var results = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            using (var conn = EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ProfileID, ProfileName FROM tblProfile";

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        results.Add(new ComboBoxItem() { Content = read.GetString(1), Tag = read.GetInt32(0) });
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            target.ItemsSource = results;
        }

        public void PopulateIncomeTypeList(ComboBox target)
        {
            var results = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            foreach (var item in Enum.GetValues(typeof(IncomeType)))
            {
                results.Add(new ComboBoxItem() { Content = item.ToString(), Tag = (int)item });
            }

            target.ItemsSource = results;
        }

        public void PopulateExpenseTypeList(ComboBox target)
        {
            var results = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            foreach (var item in Enum.GetValues(typeof(ExpenseType)))
            {
                results.Add(new ComboBoxItem() { Content = item.ToString(), Tag = (int)item });
            }

            target.ItemsSource = results;
        }

        public void PopulateAccountsList(ComboBox target, int profileId)
        {
            var results = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            using (var conn = EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT AccountID, Acc_Name FROM tblAccounts WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        results.Add(new ComboBoxItem() { Content = read.GetString(1), Tag = read.GetInt32(0) });
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            target.ItemsSource = results;
        }

        public void PopulateDebtsList(ComboBox target, int profileId)
        {
            var results = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            using (var conn = EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT DebtID, Dbt_Name FROM tblDebts WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        results.Add(new ComboBoxItem() { Content = read.GetString(1), Tag = read.GetInt32(0) });
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            target.ItemsSource = results;
        }
    }
}
