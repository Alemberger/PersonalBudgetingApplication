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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PersonalBudgetingApplication.Classes;

namespace PersonalBudgetingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string LoadedPage { get; set; } = "Default";

        public Profile Profile { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            //Apply Profile list to a dropdown list

            Common.PopulateProfileList(DDLProfileList);

            if (DDLProfileList.Items.Count < 2)
            {
                PrimaryFrame.Navigate(new Uri("StarterPage.xaml", UriKind.RelativeOrAbsolute));
                LoadedPage = "Starter";
            }
        }

        private void NavBarToggle_Click(object sender, RoutedEventArgs e)
        {
            if (NavBar.Visibility == Visibility.Collapsed)
            {
                NavBar.Visibility = Visibility.Visible;
                NavBarToggle.Content = "Hide";
            }
            else
            {
                NavBarToggle.Content = "Show";
                NavBar.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnNewProfile_Click(object sender, RoutedEventArgs e)
        {
            var Win2 = new ProfileCreation();

            Win2.Show();
        }

        private void BtnChangePage_Click(object sender, RoutedEventArgs e)
        {
            if (LoadedPage == "Default")
            {
                PrimaryFrame.Navigate(new Uri("SecondPage.xaml", UriKind.RelativeOrAbsolute));

                LoadedPage = "Second";
            }
            else
            {
                PrimaryFrame.Navigate(new Uri("DefaultPage.xaml", UriKind.RelativeOrAbsolute));

                LoadedPage = "Default";
            }
        }

        private void BtnExecuteCommands_Click(object sender, RoutedEventArgs e)
        {
            if (DDLProfileList.SelectedIndex == -1 || DDLProfileList.SelectedIndex == 0)
            {
                MessageBox.Show("Must select a profile");
                return;
            }

            var profile = new Profile(((ComboBoxItem)DDLProfileList.SelectedItem).Content.ToString());

            var Income = new IncomeEntryWindow(profile);

            Income.Show();
        }

        private void DDLProfileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = DDLProfileList.SelectedIndex;

            if (index == -1 || index == 0) { LblTitle.Content = "Overview"; return; }

            var selected = new Profile(((ComboBoxItem)DDLProfileList.Items[index]).Content.ToString());

            Profile = selected;
            LblTitle.Content = "Overview of " + Profile.ProfileName;
        }
    }
}
