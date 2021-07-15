using System;
using System.Collections.Generic;
using System.Data;
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
using PersonalBudgetingApplication.Core_Objects;

namespace PersonalBudgetingApplication
{
    /// <summary>
    /// Interaction logic for AccountCreator.xaml
    /// </summary>
    public partial class AccountCreator : Window
    {
        public Profile Profile { get; set; }

        public Account EditAccount { get; set; }

        public AccountCreator()
        {
            InitializeComponent();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //Check that the controls are filled out correctly
            if (!Validations())
            {
                _ = MessageBox.Show("Missing or invalid information entered");
                return;
            }

            //Load the entered values into the EditAccountObject
            EditAccount.Name = TbAccountName.Text.Trim();
            EditAccount.Amount = Convert.ToDouble(TbAmount.Text.Trim());
            EditAccount.Type = Convert.ToInt32(((ComboBoxItem)CmbType.SelectedItem).Tag);
            EditAccount.LastUpdateDate = DateTime.Parse(TbInitialDate.Text.Trim());
            EditAccount.RecordBy = Profile.Name;
            EditAccount.RecordDate = DateTime.Now;

            EditAccount.Save();

            CloseWindow();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private bool Validations()
        {
            bool valid = true;

            if (!ValidateAccountName())
            {
                valid = false;
            }

            if (!ValidateAccountAmount())
            {
                valid = false;
            }

            if (!ValidateAccountType())
            {
                valid = false;
            }

            if (!ValidateAccountDate())
            {
                valid = false;
            }

            return valid;
        }

        private bool ValidateAccountName()
        {
            bool valid = true; 

            if (TbAccountName.Text == "") { valid = false; }

            return valid;
        }

        private bool ValidateAccountAmount()
        {
            bool valid = true;
            if (TbAmount.Text == "") { valid = false; }

            try
            {
                double check = Convert.ToDouble(TbAmount.Text.Trim());
            }
            catch(FormatException) { valid = false; }

            return valid;
        }

        private bool ValidateAccountType()
        {
            if (CmbType.SelectedIndex < 1) { return false; }

            try
            {
                int check = Convert.ToInt32(((ComboBoxItem)CmbType.SelectedItem).Tag);
            }
            catch (FormatException) { return false; }
            
            return true;
        }

        private bool ValidateAccountDate()
        {
            if (TbInitialDate.Text == "") { return false; }

            try
            {
                DateTime check = DateTime.Parse(TbInitialDate.Text.Trim());
            }
            catch (FormatException) { return false; }

            return true;
        }

        private void CloseWindow()
        {
            if (Application.Current.MainWindow is null)
            {
                Application.Current.MainWindow = new MainWindow();
                ((MainWindow)Application.Current.MainWindow).Profile = Profile;
                Application.Current.MainWindow.Show();
            }
            else
            {
                Application.Current.MainWindow.Show();
            }

            Close();
        }
    }
}
