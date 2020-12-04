using System;
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

namespace PersonalBudgetingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Apply Profile list to a dropdown list
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
    }
}
