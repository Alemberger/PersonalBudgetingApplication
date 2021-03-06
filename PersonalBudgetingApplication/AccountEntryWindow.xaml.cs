﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
    /// Interaction logic for AccountEntryWindow.xaml
    /// </summary>
    public partial class AccountEntryWindow : Window
    {
        public Profile Profile { get; set; }

        public Account Account { get; set; }

        public AccountEntryWindow()
        {
            InitializeComponent();
        }

        public AccountEntryWindow(Profile profile)
        {
            InitializeComponent();

            Profile = profile;
        }

        public AccountEntryWindow(Profile profile, Account account)
        {
            InitializeComponent();

            Profile = profile;

            Account = account;
        }

        private void BtnSubmitAccount_Click(object sender, RoutedEventArgs e)
        {
            if (TbName.Text == "" || TbAmount.Text == "")
            {
                MessageBox.Show("Must include name and a starting amount");
                return;
            }

            if (!Common.CheckAmountInput(TbAmount.Text)) { MessageBox.Show("Must submit amount in ###.## format"); return; }

            var submission = new Account() { ID = -1, Name = TbName.Text, Amount = Convert.ToDouble(TbAmount.Text), LastUpdateDate = DateTime.Now, ProfileID = Profile.ProfileID, RecordBy = Profile.ProfileName, RecordDate = DateTime.Now };

            try
            {
                submission.SubmitAccount();
            }
            catch (DatabaseException ex)
            {
                MessageBox.Show(ex.ErrorMessage);
                return;
            }

            Common.ReturnToMainWindow(Profile);

            //Refresh the default page
            ((DefaultPage)((MainWindow)Application.Current.MainWindow).PrimaryFrame.Content).ChangeGrids(OverviewTable.Accounts);

            MessageBox.Show("Account record created");
            Close();
        }

        private void BtnCancelAccount_Click(object sender, RoutedEventArgs e)
        {
            Common.ReturnToMainWindow(Profile);
            Close();
        }
    }
}
