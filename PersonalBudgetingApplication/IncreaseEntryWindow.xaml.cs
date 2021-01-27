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
using System.Windows.Shapes;
using PersonalBudgetingApplication.Classes;

namespace PersonalBudgetingApplication
{
    /// <summary>
    /// Interaction logic for IncreaseEntryWindow.xaml
    /// </summary>
    public partial class IncreaseEntryWindow : Window
    {
        public Profile Profile { get; set; }

        public Debt Debt { get; set; }

        public IncreaseEntryWindow()
        {
            InitializeComponent(); 
            
            var pop = new ListPopulaters();

            pop.InterestTypeList(DDLIncreaseType);
        }

        public IncreaseEntryWindow(Debt selectedDebt)
        {
            InitializeComponent();

            Debt = selectedDebt;

            var pop = new ListPopulaters();

            pop.InterestTypeList(DDLIncreaseType);
        }

        public IncreaseEntryWindow(Debt selectedDebt, Profile selectedProfile)
        {
            InitializeComponent();

            Debt = selectedDebt;

            Profile = selectedProfile; 
            
            var pop = new ListPopulaters();

            pop.InterestTypeList(DDLIncreaseType);
        }

        private void BtnIncreaseSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (TbIncreaseAmount.Text == "" || TbIncreaseDate.Text == "" || DDLIncreaseType.SelectedIndex < 1) { MessageBox.Show("Missing required information"); return; }

            if (!Common.CheckAmountInput(TbIncreaseAmount.Text)) { MessageBox.Show("Please provide a valid amount"); return; }

            var submission = new DebtIncrease() { Amount = Convert.ToDouble(TbIncreaseAmount.Text), Date = DateTime.Parse(TbIncreaseDate.Text), DebtID = Debt.ID, IncreaseType = (DebtIncreaseType)Convert.ToInt32(((ComboBoxItem)DDLIncreaseType.SelectedItem).Tag), RecordBy = Profile.ProfileName, RecordDate = DateTime.Now };

            if (!submission.SubmitInterestRecord())
            {
                MessageBox.Show("Invalid data submitted");
                return;
            }

            MessageBox.Show("Increase submitted successfully");

            Common.ReturnToMainWindow(Profile);

            var main = (MainWindow)Application.Current.MainWindow;

            if (main.LoadedPage == "Default")
            {
                ((DefaultPage)main.PrimaryFrame.Content).RefreshGrid();
            }

            Close();
        }

        private void BtnIncreaseCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!(Profile is null))
            {
                Common.ReturnToMainWindow(Profile);
            }
            else
            {
                var defaults = SettingSerialization.ReadSettings();

                if (defaults.DefaultProfile is null)
                {
                    Common.ReturnToMainWindow(new Profile());
                }
                else
                {
                    Common.ReturnToMainWindow(defaults.DefaultProfile);
                }
            }

            Close();
        }
    }
}
