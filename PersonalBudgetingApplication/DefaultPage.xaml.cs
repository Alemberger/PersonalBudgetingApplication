﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PersonalBudgetingApplication.Classes;
using PersonalBudgetingApplication.Classes.Sorting;

namespace PersonalBudgetingApplication
{
    /// <summary>
    /// Interaction logic for DefaultPage.xaml
    /// </summary>
    public partial class DefaultPage : Page
    {
        public Profile Profile { get; set; }

        public OverviewTable OpenTable { get; set; } = OverviewTable.Accounts;

        public Account SelectedAccount { get; set; }

        public Debt SelectedDebt { get; set; }

        public DefaultPage()
        {
            InitializeComponent();

            var Main = (MainWindow)Application.Current.MainWindow;

            Profile = Main.Profile;

            //Apply the default table setting
            var saved = SettingSerialization.ReadSettings();

            if (saved.DefaultOverviewTable != OverviewTable.None)
            {
                ChangeGrids(saved.DefaultOverviewTable);
            }
            else
            {
                ChangeGrids(OverviewTable.Accounts);
            }
        }

        public void ChangeGrids(OverviewTable target)
        {
            switch (target)
            {
                case OverviewTable.Accounts:
                    LblTitle.Content = "Accounts";
                    BtnNewOption.Content = "New Account";
                    OpenTable = OverviewTable.Accounts;

                    GvAccounts.Visibility = Visibility.Visible;
                    GvDebts.Visibility = Visibility.Hidden;

                    BtnChangeGrid.Tag = "Debt";
                    BtnChangeGrid.Content = "Debts";

                    BtnEnterPositive.Content = "Enter Income";
                    BtnEnterPositive.Tag = "Account";
                    BtnEnterNegative.Content = "Enter Expense";
                    BtnEnterNegative.Tag = "Account";

                    DDLOptions.ItemsSource = Profile.ListAccounts();

                    break;
                case OverviewTable.Debts:
                    LblTitle.Content = "Debts";
                    BtnNewOption.Content = "New Debt";
                    OpenTable = OverviewTable.Debts;

                    GvAccounts.Visibility = Visibility.Hidden;
                    GvDebts.Visibility = Visibility.Visible;

                    BtnChangeGrid.Tag = "Account";
                    BtnChangeGrid.Content = "Accounts";

                    BtnEnterPositive.Content = "Enter Payment";
                    BtnEnterPositive.Tag = "Debt";
                    BtnEnterNegative.Content = "Enter Increase";
                    BtnEnterNegative.Tag = "Debt";

                    DDLOptions.ItemsSource = Profile.ListDebts();

                    break;
            }
            GvAccounts.ItemsSource = GvDebts.ItemsSource = new DataTable().Rows;
        }

        private void BtnChangeGrid_Click(object sender, RoutedEventArgs e)
        {
            switch (BtnChangeGrid.Tag.ToString())
            {
                case "Debt":
                    ChangeGrids(OverviewTable.Debts);
                    break;
                case "Account":
                    ChangeGrids(OverviewTable.Accounts);
                    break;
            }
        }

        private void BtnEnterPositive_Click(object sender, RoutedEventArgs e)
        {
            if (DDLOptions.SelectedIndex == -1 || DDLOptions.SelectedIndex == 0)
            {
                string message = "Must select a";
                if (OpenTable == OverviewTable.Accounts)
                {
                    message += "n account.";
                }
                else if (OpenTable == OverviewTable.Debts)
                {
                    message += " debt.";
                }

                MessageBox.Show(message);
                return;
            }

            if (OpenTable == OverviewTable.Accounts)
            {
                var window = new IncomeEntryWindow(SelectedAccount, Profile);

                window.Show();
            }
            else if (OpenTable == OverviewTable.Debts)
            {
                var window = new PaymentEntryWindow(SelectedDebt, Profile);

                window.Show();
            }
            else { throw new Exception("Unknown Table"); }
        }

        private void BtnEnterNegative_Click(object sender, RoutedEventArgs e)
        {
            if (DDLOptions.SelectedIndex == -1 || DDLOptions.SelectedIndex == 0)
            {
                string message = "Must select a";
                if (OpenTable == OverviewTable.Accounts)
                {
                    message += "n account.";
                }
                else if (OpenTable == OverviewTable.Debts)
                {
                    message += " debt.";
                }

                MessageBox.Show(message);
                return;
            }

            if (OpenTable == OverviewTable.Accounts)
            {
                var window = new ExpenseEntryWindow(SelectedAccount, Profile);

                window.Show();
            }
            else if (OpenTable == OverviewTable.Debts)
            {
                var window = new IncreaseEntryWindow(SelectedDebt, Profile);

                window.Show();
            }
            else { throw new Exception("Unknown Table"); }
        }

        private void BtnRefreshGrids_Click(object sender, RoutedEventArgs e)
        {
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            if (OpenTable == OverviewTable.Accounts)
            {
                if (DDLOptions.SelectedIndex > 0)
                {
                    GvAccounts.ItemsSource = new AccountOverviewTable(SelectedAccount).Items;
                }
            }
            else if (OpenTable == OverviewTable.Debts)
            {
                if (DDLOptions.SelectedIndex > 0)
                {
                    GvDebts.ItemsSource = new DebtOverviewTable(SelectedDebt).Items;
                }
            }
        }

        private void BtnNewOption_Click(object sender, RoutedEventArgs e)
        {
            if (OpenTable == OverviewTable.Accounts)
            {
                var window = new AccountEntryWindow(Profile);

                window.Show();
            }
            else if (OpenTable == OverviewTable.Debts)
            {
                var window = new DebtEntryWindow(Profile);

                window.Show();
            }
        }

        private void DDLOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OpenTable == OverviewTable.Accounts)
            {
                if (DDLOptions.SelectedIndex < 1)
                {
                    BtnNewOption.Content = "New Account";
                    SelectedAccount = null;
                    GvAccounts.ItemsSource = new DataTable().Rows;
                }
                else
                {
                    BtnNewOption.Content = "Edit Account";
                    SelectedAccount = new Account(Convert.ToInt32(((ComboBoxItem)DDLOptions.SelectedItem).Tag));
                    GvAccounts.ItemsSource = new AccountOverviewTable(SelectedAccount).Items;
                }
            }
            else
            {
                if (DDLOptions.SelectedIndex < 1)
                {
                    BtnNewOption.Content = "New Debt";
                    GvDebts.ItemsSource = new DataTable().Rows;
                    SelectedDebt = null;
                }
                else
                {
                    BtnNewOption.Content = "Edit Debt";
                    SelectedDebt = new Debt(Convert.ToInt32(((ComboBoxItem)DDLOptions.SelectedItem).Tag));
                    GvDebts.ItemsSource = new DebtOverviewTable(SelectedDebt).Items;
                }
            }
        }
    }
}
