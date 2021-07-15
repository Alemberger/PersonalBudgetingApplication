using PersonalBudgetingApplication.Classes;
using PersonalBudgetingApplication.Core_Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PersonalBudgetingApplication
{
    /// <summary>
    /// Interaction logic for DefaultPage.xaml
    /// </summary>
    public partial class DefaultPage : Page
    {
        public Profile Profile { get; set; }

        public OverviewTable OpenTable { get; set; } = OverviewTable.Accounts;

        public Account Selected { get; set; }

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

        private List<ComboBoxItem> FormatOptionList(List<Account> sourceList)
        {
            List<ComboBoxItem> bindList = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "", IsSelected = true } };
            foreach (Account account in sourceList)
            {
                bindList.Add(new ComboBoxItem() { Content = account.Name, Tag = account.AccountID.ToString() });
            }
            return bindList;
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

                    DDLOptions.ItemsSource = FormatOptionList(Profile.GetAccounts());

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

                    DDLOptions.ItemsSource = FormatOptionList(Profile.GetDebts());

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
            throw new NotImplementedException();
        }

        private void BtnEnterNegative_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnRefreshGrids_Click(object sender, RoutedEventArgs e)
        {
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            throw new NotImplementedException();
        }

        private void BtnNewOption_Click(object sender, RoutedEventArgs e)
        {
            if (OpenTable == OverviewTable.Accounts)
            {
                AccountCreator newWindow = new AccountCreator();

                newWindow.Profile = Profile;

                newWindow.EditAccount = new Account() { State = System.Data.Entity.EntityState.Added };

                newWindow.Show();
            }
            else if (OpenTable == OverviewTable.Debts)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new InvalidOperationException("Cannot add a new option without selecting an appropriate table type");
            }
        }

        private void DDLOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Display the selected Options Information in the appropriate table
        }
    }
}
