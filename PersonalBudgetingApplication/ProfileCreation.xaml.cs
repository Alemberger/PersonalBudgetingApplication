using System;
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
    /// Interaction logic for ProfileCreation.xaml
    /// </summary>
    public partial class ProfileCreation : Window
    {
        public ProfileCreation()
        {
            InitializeComponent();
        }

        private void BtnCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            if (TbProfileName.Text.Trim() == "")
            {
                MessageBox.Show("Must provide a Profile Name");
                return;
            }

            //Confirm the profile name is not already in use.
            var profileSelect = "SELECT ProfileID, ProfileName FROM tblProfile";
            var profileColumns = new List<DataColumn> { new DataColumn("ProfileID", typeof(int)), new DataColumn("ProfileName", typeof(string)) };

            var profiles = DataAccess.ExecuteSelectQuery(profileSelect, profileColumns);

            foreach (DataRow row in profiles.Rows)
            {
                if (row[1].ToString().Trim().ToLower() == TbProfileName.Text.ToLower())
                {
                    MessageBox.Show("Profile name is already in use");
                    return;
                }
            }

            //Create the profile object and submit it to the database
            var toSubmit = new Core_Objects.Profile() { Name = TbProfileName.Text.Trim(), RecordBy = TbProfileName.Text.Trim(), RecordDate = DateTime.Now };
            toSubmit.SaveChanges();

            //Return to the main window and select the newly created profile
            if (!Application.Current.Windows.OfType<MainWindow>().Any())
            {
                var New = new MainWindow();

                foreach (ComboBoxItem item in New.DDLProfileList.Items)
                {
                    if (item.Content.ToString() == toSubmit.Name) { item.IsSelected = true; break; }
                }

                New.Show();
            }
            else
            {
                var Main = (MainWindow)Application.Current.MainWindow;

                Common.PopulateProfileList(Main.DDLProfileList);

                foreach (ComboBoxItem item in Main.DDLProfileList.Items)
                {
                    if ((string)item.Content == toSubmit.Name)
                    {
                        item.IsSelected = true;
                        break;
                    }
                }

                Main.Show(); 
            }
            Close();
        }

        private void BtnCancelProfile_Click(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<MainWindow>().Any())
            {
                var New = new MainWindow();

                New.Show();
            }
            else
            {
                var Main = (MainWindow)Application.Current.MainWindow;
                Main.Show();
                Main.Focus();
            }
            Close();
        }
    }
}
