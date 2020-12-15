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
    /// Interaction logic for ExpenseEntryWidnow.xaml
    /// </summary>
    public partial class ExpenseEntryWindow : Window
    {
        public Profile Profile { get; set; }

        public ExpenseEntryWindow()
        {
            InitializeComponent();

            Common.PopulateExpenseTypeList(DDLExpenseType);
        }

        public ExpenseEntryWindow(Profile profile)
        {
            InitializeComponent();

            Common.PopulateExpenseTypeList(DDLExpenseType);

            Profile = profile;
        }

        private void BtnExpenseEntrySubmit_Click(object sender, RoutedEventArgs e)
        {
            if (TbExpenseAmount.Text == "" | DDLExpenseType.SelectedIndex < 1 | TbExpenseDate.Text == "")
            {
                MessageBox.Show("Must submit an amount, type, and date");
                return;
            }

            var amount = TbExpenseAmount.Text.Trim();

            if (!Common.CheckAmountInput(amount)) { MessageBox.Show("Invalid amount provided. Please provide in format ###.##"); return; }

            var entry = new ExpenseEntry() { Amount = Convert.ToDouble(amount), Type = (ExpenseType)Convert.ToInt32(((ComboBoxItem)DDLExpenseType.SelectedItem).Tag), Date = TbExpenseDate.Text };

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO tblExpense (ProfileID, Exp_Amount, Exp_Type, Exp_Date) VALUES (@ProfileID, @Amount, @Type, @Date)";
                    cmd.Parameters.AddWithValue("@ProfileID", Profile.ProfileID);
                    cmd.Parameters.AddWithValue("@Amount", entry.Amount);
                    cmd.Parameters.AddWithValue("@Type", (int)entry.Type);
                    cmd.Parameters.AddWithValue("@Date", entry.Date);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            MessageBox.Show("Expense entry submitted");

            Common.ReturnToMainWindow(Profile);

            Close();
        }

        private void BtnExpenseEntryCancel_Click(object sender, RoutedEventArgs e)
        {
            Common.ReturnToMainWindow(Profile);

            Close();
        }
    }
}
