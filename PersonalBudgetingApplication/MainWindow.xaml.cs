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
        private string _LoadedPage = "Default";

        public MainWindow()
        {
            InitializeComponent();

            //Apply Profile list to a dropdown list

            Common.PopulateProfileList(DDLProfileList);
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

        private void BtnOpenWindow_Click(object sender, RoutedEventArgs e)
        {
            var Win2 = new ProfileCreation();

            Win2.Show();
        }

        private void BtnChangePage_Click(object sender, RoutedEventArgs e)
        {
            if (_LoadedPage == "Default")
            {
                PrimaryFrame.Navigate(new Uri("SecondPage.xaml", UriKind.RelativeOrAbsolute));

                _LoadedPage = "Second";
            }
            else
            {
                PrimaryFrame.Navigate(new Uri("DefaultPage.xaml", UriKind.RelativeOrAbsolute));

                _LoadedPage = "Default";
            }
        }
    }
}
