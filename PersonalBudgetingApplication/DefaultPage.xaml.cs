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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PersonalBudgetingApplication.Classes;

namespace PersonalBudgetingApplication
{
    /// <summary>
    /// Interaction logic for DefaultPage.xaml
    /// </summary>
    public partial class DefaultPage : Page
    {
        public Profile Profile { get; set; }

        public List<IncomeEntry> IncomeEntries { get { return new IncomeEntries(Profile); } }

        public List<ExpenseEntry> ExpenseEntries { get { return new ExpenseEntries(Profile); } }

        public List<SavingsEntry> SavingsEntries { get { return new SavingsEntries(Profile); } }

        public DefaultPage()
        {
            InitializeComponent();

            var Main = (MainWindow)Application.Current.MainWindow;

            Profile = Main.Profile;

            GvIncome.ItemsSource = IncomeEntries;
            GvExpenses.ItemsSource = ExpenseEntries;
            GvSavings.ItemsSource = SavingsEntries;

            //Apply the default table setting
            var saved = SettingSerialization.ReadSettings();

            switch (saved.DefaultOverviewTable)
            {
                case OverviewTable.None:
                    break;
                case OverviewTable.Income:
                    LblTitle.Content = "Income";
                    BtnEnterRecord.Content = "Enter Income";
                    BtnEnterRecord.Tag = "Income";

                    GvSavings.Visibility = Visibility.Hidden;
                    GvIncome.Visibility = Visibility.Visible;
                    GvExpenses.Visibility = Visibility.Hidden;

                    BtnChangeGrid.Tag = "Expense";
                    BtnChangeGrid.Content = "Expense";
                    BtnEnterRecord.Content = "Enter Income";
                    break;
                case OverviewTable.Expense:
                    LblTitle.Content = "Expense";
                    BtnEnterRecord.Content = "Enter Expense";
                    BtnEnterRecord.Tag = "Expense";

                    GvSavings.Visibility = Visibility.Hidden;
                    GvIncome.Visibility = Visibility.Hidden;
                    GvExpenses.Visibility = Visibility.Visible;

                    BtnChangeGrid.Tag = "Savings";
                    BtnChangeGrid.Content = "Savings";
                    break;
                case OverviewTable.Savings:
                    LblTitle.Content = "Savings";
                    BtnEnterRecord.Content = "Enter Savings";
                    BtnEnterRecord.Tag = "Savings";

                    GvSavings.Visibility = Visibility.Visible;
                    GvIncome.Visibility = Visibility.Hidden;
                    GvExpenses.Visibility = Visibility.Hidden;

                    BtnChangeGrid.Tag = "Income";
                    BtnChangeGrid.Content = "Income";
                    break;
            }
        }

        private List<IncomeEntry> GatherIncomeRecords(int profileId)
        {
            var records = new List<IncomeEntry>();

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT IncomeId, Inc_Amount, Inc_Type, Inc_Date FROM tblIncome WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == System.Data.ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new IncomeEntry()
                        {
                            ProfileId = profileId,
                            IncomeID = read.GetInt32(0),
                            Amount = read.GetDouble(1),
                            Date = DateTime.Parse(read.GetString(3)),
                            Type = (IncomeType)read.GetInt32(2)
                        };

                        records.Add(entry);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); } 
            }

            

            return records;
        }

        private List<ExpenseEntry> GatherExpenseRecords(int profileId)
        {
            var records = new List<ExpenseEntry>();

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ExpenseID, Exp_Amount, Exp_Type, Exp_Date FROM tblExpense WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == System.Data.ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new ExpenseEntry()
                        {
                            ExpenseId = read.GetInt32(0),
                            ProfileId = profileId,
                            Amount = read.GetDouble(1),
                            Type = (ExpenseType)read.GetInt32(2),
                            Date = DateTime.Parse(read.GetString(3))
                        };

                        records.Add(entry);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return records;
        }

        private void BtnChangeGrid_Click(object sender, RoutedEventArgs e)
        {
            switch (BtnChangeGrid.Tag)
            {
                case "Expense":
                    //Change the grid to the expense report
                    //Update the control text to represent the expense related items
                    LblTitle.Content = "Expense";
                    BtnEnterRecord.Content = "Enter Expense";
                    BtnEnterRecord.Tag = "Expense";

                    //Hide the non-expense Grids and show the expense grid
                    GvSavings.Visibility = Visibility.Hidden;
                    GvIncome.Visibility = Visibility.Hidden;
                    GvExpenses.Visibility = Visibility.Visible;

                    //Set the change grid button to load savings next
                    BtnChangeGrid.Tag = "Savings";
                    BtnChangeGrid.Content = "Savings";

                    break;
                case "Income":
                    //Change the grid to the income report
                    LblTitle.Content = "Income";
                    BtnEnterRecord.Content = "Enter Income";
                    BtnEnterRecord.Tag = "Income";

                    GvSavings.Visibility = Visibility.Hidden;
                    GvIncome.Visibility = Visibility.Visible;
                    GvExpenses.Visibility = Visibility.Hidden;

                    BtnChangeGrid.Tag = "Expense";
                    BtnChangeGrid.Content = "Expense";
                    BtnEnterRecord.Content = "Enter Income";
                    break;
                case "Savings":
                    //Change the grid to the savings report
                    //Update the control text to represent the savings related items
                    LblTitle.Content = "Savings";
                    BtnEnterRecord.Content = "Enter Savings";
                    BtnEnterRecord.Tag = "Savings";

                    //Hide the non-savings grids and show the savings grid
                    GvSavings.Visibility = Visibility.Visible;
                    GvIncome.Visibility = Visibility.Hidden;
                    GvExpenses.Visibility = Visibility.Hidden;


                    //Set the change grid button to load income next
                    BtnChangeGrid.Tag = "Income";
                    BtnChangeGrid.Content = "Income";
                    break;
                default:
                    throw new Exception("Unknown grid target");
            }
        }

        private void BtnEnterRecord_Click(object sender, RoutedEventArgs e)
        {
            if (Profile is null) { MessageBox.Show("Select a profile"); return; }

            switch (BtnEnterRecord.Tag)
            {
                case "Expense":
                    var expenseFound = false;

                    foreach (Window check in Application.Current.Windows)
                    {
                        if (check.GetType() == typeof(ExpenseEntryWindow))
                        {
                            check.Show();
                            check.Focus();
                            expenseFound = true;
                        }
                    }

                    if (!expenseFound)
                    {
                        var window = new ExpenseEntryWindow(Profile);
                        window.Show();
                    }

                    break;
                case "Income":
                    var incomeFound = false;

                    foreach (Window check in Application.Current.Windows)
                    {
                        if (check.GetType() == typeof(IncomeEntryWindow))
                        {
                            check.Show();
                            check.Focus();
                            incomeFound = true;
                        }
                    }

                    if (!incomeFound)
                    {
                        var window = new IncomeEntryWindow(Profile);
                        window.Show();
                    }

                    break;
                case "Savings":
                    var savingsFound = false;

                    foreach (Window check in Application.Current.Windows)
                    {
                        if (check.GetType() == typeof(SavingsEntryWindow))
                        {
                            check.Show();
                            check.Focus();
                            savingsFound = true;
                        }
                    }

                    if (!savingsFound)
                    {
                        var window = new SavingsEntryWindow(Profile);
                        window.Show();
                    }

                    break;
            }
        }

        private void BtnRefreshGrids_Click(object sender, RoutedEventArgs e)
        {
            GvExpenses.ItemsSource = ExpenseEntries;
            GvIncome.ItemsSource = IncomeEntries;
            GvSavings.ItemsSource = SavingsEntries;
        }

        private void GvExpenses_Sorting(object sender, DataGridSortingEventArgs e)
        {
            string direction = e.Column.SortDirection.ToString();

            if (direction == "") { direction = "Ascending"; }
            else if (direction == "Ascending") { direction = "Descending"; }
            else if (direction == "Descending") { direction = "Ascending"; }

            MessageBox.Show(string.Format("sorting grid by '{0}' column in {1} order", e.Column.SortMemberPath, direction));
        }
    }
}
