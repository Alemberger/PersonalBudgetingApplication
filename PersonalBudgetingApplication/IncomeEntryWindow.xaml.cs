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
    /// Interaction logic for IncomeEntry.xaml
    /// </summary>
    public partial class IncomeEntryWindow : Window
    {
        public Profile Profile { get; set; }

        public IncomeEntryWindow()
        {
            InitializeComponent();

            Common.PopulateIncomeTypeList(DDLIncomeType);
        }

        public IncomeEntryWindow(Profile profile)
        {
            InitializeComponent();

            Common.PopulateIncomeTypeList(DDLIncomeType);

            Profile = profile;
        }

        private void BtnIncomeEntrySubmit_Click(object sender, RoutedEventArgs e)
        {
            //Check that both the Amount and Type were provided
            if (TbIncomeAmount.Text == "" | DDLIncomeType.SelectedIndex == 0 | DDLIncomeType.SelectedIndex == -1)
            {
                MessageBox.Show("Must provide both an amount and a type");
                return;
            }

            var amount = TbIncomeAmount.Text.Trim();

            //Check the Amount string for validity
            Regex check = new Regex("^[0-9]+\\.[0-9]{2}$");
            if (!check.IsMatch(amount)) { MessageBox.Show("Invalid amount provided. Please provide in format ###.##"); return; }

            var entry = new IncomeEntry(Convert.ToDouble(amount), ((ComboBoxItem)DDLIncomeType.SelectedItem).Content.ToString(), TbIncomeDate.Text); ;

            //Submit the record
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO tblIncome (ProfileID, Inc_Amount, Inc_Type, Inc_Date) VALUES (@ProfileID, @Amount, @Type, @Date)";
                    cmd.Parameters.AddWithValue("@ProfileID", Profile.ProfileID);
                    cmd.Parameters.AddWithValue("@Amount", entry.Amount);
                    cmd.Parameters.AddWithValue("@Type", (int)entry.Type);
                    cmd.Parameters.AddWithValue("@Date", entry.Date);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            MessageBox.Show("Income entry submitted");

            Common.ReturnToMainWindow(Profile);

            Close();
        }

        private void BtnIncomeEntryCancel_Click(object sender, RoutedEventArgs e)
        {
            Common.ReturnToMainWindow(Profile);

            Close();
        }
    }
}
