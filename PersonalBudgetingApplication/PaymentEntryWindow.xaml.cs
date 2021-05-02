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
    /// Interaction logic for PaymentEntryWindow.xaml
    /// </summary>
    public partial class PaymentEntryWindow : Window
    {
        public Profile Profile { get; set; }

        public Debt Debt { get; set; }

        public PaymentEntryWindow()
        {
            InitializeComponent();
        }

        public PaymentEntryWindow(Debt selectedDebt)
        {
            InitializeComponent();

            Debt = selectedDebt;
        }

        public PaymentEntryWindow(Debt selectedDebt, Profile selectedProfile)
        {
            InitializeComponent();

            Profile = selectedProfile;

            Debt = selectedDebt;
        }

        private void BtnPaymentSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (TbPaymentAmount.Text == "" || TbPaymentDate.Text == "") { MessageBox.Show("Missing required information"); return; }

            if (!Common.CheckAmountInput(TbPaymentAmount.Text)) { MessageBox.Show("Invalid amount provided"); return; }

            var submission = new DebtPayment() { DebtID = Debt.ID, Amount = Convert.ToDouble(TbPaymentAmount.Text), Date = DateTime.Parse(TbPaymentDate.Text), RecordBy = Profile.ProfileName, RecordDate = DateTime.Now };

            try
            {
                submission.SubmitDebtPayment();
            }
            catch (DatabaseException ex) { MessageBox.Show(ex.ErrorMessage); return; }

            Common.ReturnToMainWindow(Profile);

            var main = (MainWindow)Application.Current.MainWindow;

            if (main.LoadedPage == "Default")
            {
                ((DefaultPage)main.PrimaryFrame.Content).RefreshGrid();
            }

            MessageBox.Show("Payment successfully submitted.");

            Close();
        }

        private void BtnPaymentCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Common.ReturnToMainWindow(Profile);
            }
            catch (NullReferenceException) { };

            Close();
        }
    }
}
