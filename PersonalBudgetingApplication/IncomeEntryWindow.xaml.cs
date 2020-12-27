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
        public Account Account { get; set; }
        
        public Profile Profile { get; set; }

        public IncomeEntryWindow()
        {
            InitializeComponent();

            var populater = new ListPopulaters();

            populater.PopulateIncomeTypeList(DDLIncomeType);
        }

        public IncomeEntryWindow(Account account)
        {
            InitializeComponent();

            var populater = new ListPopulaters();

            populater.PopulateIncomeTypeList(DDLIncomeType);

            Account = account;
        }

        public IncomeEntryWindow(Account account, Profile profile)
        {
            InitializeComponent();

            var populater = new ListPopulaters();

            populater.PopulateIncomeTypeList(DDLIncomeType);

            Account = account;

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

            var entry = new Income() { AccountID = Account.ID, Amount = Convert.ToDouble(amount), Type = (IncomeType)Convert.ToInt32(((ComboBoxItem)DDLIncomeType.SelectedItem).Tag), Date = DateTime.Parse(TbIncomeDate.Text), RecordBy = Profile.ProfileName, RecordDate = DateTime.Now } ;

            //Submit the record
            entry.SubmitRecord();

            Account.UpdateAccountBalance(entry.Amount, entry.Date);

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
