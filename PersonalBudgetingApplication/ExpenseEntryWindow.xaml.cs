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
        public Account Account { get; set; }

        public Profile Profile { get; set; }

        public ExpenseEntryWindow()
        {
            InitializeComponent();

            var populater = new ListPopulaters();

            populater.PopulateExpenseTypeList(DDLExpenseType);
        }

        public ExpenseEntryWindow(Account account)
        {
            InitializeComponent();

            var populater = new ListPopulaters();

            populater.PopulateExpenseTypeList(DDLExpenseType);

            Account = account;
        }

        public ExpenseEntryWindow(Account account, Profile profile)
        {
            InitializeComponent();

            var populater = new ListPopulaters();

            populater.PopulateExpenseTypeList(DDLExpenseType);

            Account = account;

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

            var entry = new Expense() { AccountID = Account.ID, Amount = Convert.ToDouble(amount), Type = (ExpenseType)Convert.ToInt32(((ComboBoxItem)DDLExpenseType.SelectedItem).Tag), Date = DateTime.Parse(TbExpenseDate.Text), RecordBy = Profile.ProfileName, RecordDate = DateTime.Now };

            entry.SubmitRecord();

            Account.UpdateAccountBalance(entry.Amount * -1.00, entry.Date);

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
