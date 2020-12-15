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
    /// Interaction logic for AdjustSettingsWindow.xaml
    /// </summary>
    public partial class AdjustSettingsWindow : Window
    {
        public AdjustSettingsWindow()
        {
            InitializeComponent();

            Common.PopulateProfileList(DDLDefaultProfile);

            PopulateOverviewTableList(DDLDefaultOverviewTable);

            var applied = SettingSerialization.ReadSettings();

            ApplySettings(applied);
        }

        private void ApplySettings(ApplicationSettings loaded)
        {
            if (!(loaded.DefaultProfile is null))
            {
                foreach (ComboBoxItem item in DDLDefaultProfile.Items)
                {
                    if (item.Content.ToString() == loaded.DefaultProfile.ProfileName)
                    {
                        DDLDefaultProfile.SelectedItem = item;
                    }
                }
            }

            if (!(loaded.DefaultOverviewTable == OverviewTable.None))
            {
                foreach (ComboBoxItem item in DDLDefaultOverviewTable.Items)
                {
                    if (item.Content.ToString() == loaded.DefaultOverviewTable.ToString())
                    {
                        DDLDefaultOverviewTable.SelectedItem = item;
                    }
                }
            }
        }

        private void PopulateOverviewTableList(ComboBox target)
        {
            var binder = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" }, new ComboBoxItem() { Content = "Income", Tag = "0" }, new ComboBoxItem() { Content = "Expense", Tag = "1" }, new ComboBoxItem() { Content = "Savings", Tag = "2" } };

            target.ItemsSource = binder;
        }

        private void BtnCancelSettings_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            var Settings = new ApplicationSettings();

            if (DDLDefaultProfile.SelectedIndex > 0)
            {
                Settings.DefaultProfile = new Profile(((ComboBoxItem)DDLDefaultProfile.SelectedItem).Content.ToString());
            }

            if (DDLDefaultOverviewTable.SelectedIndex <= 0)
            {
                Settings.DefaultOverviewTable = OverviewTable.None;
            }
            else
            {
                Settings.DefaultOverviewTable = (OverviewTable)Convert.ToInt32(((ComboBoxItem)DDLDefaultOverviewTable.SelectedItem).Tag);
            }

            SettingSerialization.SaveSettings(Settings);
            Close();
        }
    }
}
