using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PersonalBudgetingApplication.Classes;

namespace PersonalBudgetingApplication
{
    /// <summary>
    /// Interaction logic for SavingsEntryWindow.xaml
    /// </summary>
    public partial class SavingsEntryWindow : Window
    {
        public Profile Profile { get; set; }

        public SavingsEntryWindow()
        {
            InitializeComponent();
        }

        public SavingsEntryWindow(Profile profile)
        {
            InitializeComponent();

            Profile = profile;
        }

        private void BtnSavingsSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (TbSavingsAmount.Text == "" || TbSavingsDate.Text == "")
            {
                MessageBox.Show("Please provide both an amount and a date");
                return;
            }

            var amount = TbSavingsAmount.Text.Trim();

            if (!Common.CheckAmountInput(amount)) { MessageBox.Show("Please provide the amount in this format: ###.##"); return; }

            var entry = new SavingsEntry() { Amount = Convert.ToDouble(amount), Date = TbSavingsDate.Text };

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO tblSavings (ProfileID, Sav_Amount, Sav_Date) VALUES (@ProfileID, @Amount, @Date)";
                    cmd.Parameters.AddWithValue("@ProfileID", Profile.ProfileID);
                    cmd.Parameters.AddWithValue("@Amount", entry.Amount);
                    cmd.Parameters.AddWithValue("@Date", entry.Date);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            MessageBox.Show("Savings entry submitted");

            Common.ReturnToMainWindow(Profile);

            Close();
        }

        private void BtnSavingsCancel_Click(object sender, RoutedEventArgs e)
        {
            Common.ReturnToMainWindow(Profile);

            Close();
        }
    }
}
