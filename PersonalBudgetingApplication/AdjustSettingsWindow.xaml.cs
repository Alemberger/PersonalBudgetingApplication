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

            var applied = SettingSerialization.ReadSettings();

            if(!(applied.DefaultProfile is null))
            {
                foreach (ComboBoxItem item in DDLDefaultProfile.Items)
                {
                    if (item.Content.ToString() == applied.DefaultProfile.ProfileName)
                    {
                        DDLDefaultProfile.SelectedItem = item;
                    }
                }
            }
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

            SettingSerialization.SaveSettings(Settings);
            Close();
        }
    }
}
