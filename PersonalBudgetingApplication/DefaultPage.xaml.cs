using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PersonalBudgetingApplication.Classes;

namespace PersonalBudgetingApplication
{
    /// <summary>
    /// Interaction logic for DefaultPage.xaml
    /// </summary>
    public partial class DefaultPage : Page
    {
        public DefaultPage()
        {
            InitializeComponent();
        }

        private List<IncomeEntry> GatherIncomeRecords(int profileId)
        {
            var records = new List<IncomeEntry>();

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT IncomeId, Inc_Amount, Inc_Type, Inc_Date FROM tblIncome WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == System.Data.ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new IncomeEntry()
                        {
                            ProfileId = profileId,
                            IncomeID = read.GetInt32(0),
                            Amount = read.GetDouble(1),
                            Date = read.GetString(3),
                            Type = (IncomeType)read.GetInt32(2)
                        };

                        records.Add(entry);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); } 
            }

            return records;
        }

        private List<SavingsEntry> GatherSavingsRecords(int profileId)
        {
            var records = new List<SavingsEntry>();

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT SavingsID, Sav_Amount, Sav_Date FROM tblSaving WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == System.Data.ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new SavingsEntry()
                        {
                            ProfileId = profileId,
                            SavingsId = read.GetInt32(0),
                            Amount = read.GetDouble(1),
                            Date = read.GetString(2)
                        };

                        records.Add(entry);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return records;
        }

        private List<ExpenseEntry> GatherExpenseRecords(int profileId)
        {
            var records = new List<ExpenseEntry>();

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ExpenseID, Exp_Amount, Exp_Type, Exp_Date FROM tblExpense WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == System.Data.ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new ExpenseEntry()
                        {
                            ExpenseId = read.GetInt32(0),
                            ProfileId = profileId,
                            Amount = read.GetDouble(1),
                            Type = (ExpenseType)read.GetInt32(2),
                            Date = read.GetString(3)
                        };

                        records.Add(entry);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return records;
        }
    }
}
