using System;
using System.Collections.Generic;
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
    /// Interaction logic for DebtEntryWindow.xaml
    /// </summary>
    public partial class DebtEntryWindow : Window
    {
        public Profile Profile { get; set; }

        private DebtEntryWindow()
        {
            InitializeComponent();

            InitializeLists();
        }

        public DebtEntryWindow(Profile profile)
        {
            InitializeComponent();

            Profile = profile;

            InitializeLists();
        }

        private void InitializeLists()
        {
            var populate = new ListPopulaters();

            populate.InterestTypeList(DDLInterestType);

            populate.CompoundingFrequencyList(DDLCompoundingFrequency);
        }

        private void BtnDebtSubmit_Click(object sender, RoutedEventArgs e)
        {
            //Validation
            try
            {
                if (TbName.Text == "") { MessageBox.Show("Must provide a name"); return; }
                if (TbPrincipal.Text == "") { MessageBox.Show("Must provide an initial amount"); return; }
                if (TbDateOpened.Text == "") { MessageBox.Show("Must providea an opening date"); return; }
                if (TbAPR.Text == "") { MessageBox.Show("Must providea an APR"); return; }
                if (DDLInterestType.SelectedIndex < 1) { MessageBox.Show("Must select an interest type"); return; }
            }
            catch (NullReferenceException) { MessageBox.Show("Please fill out all fields"); return; }

            if (!Common.CheckAmountInput(TbPrincipal.Text)) { MessageBox.Show("Please provide the principal in format ###.##"); return; }

            //Check the APR provided
            var validApr = new Regex(@"^[0-9]+\.[0-9]+%$|^[0-9]+%$");

            if (!validApr.IsMatch(TbAPR.Text)) { MessageBox.Show("Provide APR in format ##.##%"); return; }

            if ((int)((ComboBoxItem)DDLInterestType.SelectedItem).Tag == 3)
            {
                if (DDLCompoundingFrequency.SelectedIndex < 1) { MessageBox.Show("Must select a compounding frequency for compound interest"); return; }
            }

            //Submit record
            var sApr = TbAPR.Text.Trim();
            sApr = sApr.Substring(0, sApr.Length - 1);
            var apr = Convert.ToDouble(sApr) / 100.0;

            var entry = new Debt(Profile) { Name = TbName.Text, Principal = Convert.ToDouble(TbPrincipal.Text), LastUpdateDate = DateTime.Parse(TbDateOpened.Text), InterestType = (InterestType)((int)((ComboBoxItem)DDLInterestType.SelectedItem).Tag), TimesApplied = (CompoundNumberApplied)(int)((ComboBoxItem)DDLCompoundingFrequency.SelectedItem).Tag, AnnualPercentageRate = apr, RecordBy = Profile.ProfileName, RecordDate = DateTime.Now };

            entry.SubmitDebt();

            //Cleanup
            Common.ReturnToMainWindow(Profile);

            //Refresh the default page
            ((DefaultPage)((MainWindow)Application.Current.MainWindow).PrimaryFrame.Content).ChangeGrids(OverviewTable.Debts);

            MessageBox.Show("Debt record created");

            Close();
        }

        private void BtnDebtCancel_Click(object sender, RoutedEventArgs e)
        {
            Common.ReturnToMainWindow(Profile);

            Close();
        }
    }
}
