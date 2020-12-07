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
            var conn = Common.CreateConnection();

            var cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO tblProfile (ProfileName) VALUES ('" + TbProfileName.Text + "')";

            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            cmd.ExecuteNonQuery();

            MessageBox.Show("Profile created");

            if (!Application.Current.Windows.OfType<MainWindow>().Any())
            {
                var New = new MainWindow();

                New.Show();
            }
            else
            {
                var Main = Application.Current.Windows.OfType<MainWindow>().First();

                Common.PopulateProfileList(Main.DDLProfileList);
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

                Common.PopulateProfileList(Main.DDLProfileList);
            }
            Close();
        }
    }
}
