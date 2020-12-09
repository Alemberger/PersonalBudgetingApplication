using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
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
using System.Xml.Serialization;
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

        private bool CheckedSettings { get; set; } = false;

        public MainWindow()
        {
            InitializeComponent();

            if (!CheckedSettings)
            {
                SettingSerialization.ReadSettings();
                CheckedSettings = true;
            }

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
            //Test the serializer
            var test = new SettingSerialization();

            test.SerializeFile();

            var example = test.ReadXMLFile();

            MessageBox.Show(example.ProfileName + ":" + example.ProfileID);
        }

        private void DDLProfileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = DDLProfileList.SelectedIndex;

            if (index == -1 || index == 0) 
            { 
                LblTitle.Content = "Overview"; 
                return; 
            }

            var selected = new Profile(((ComboBoxItem)DDLProfileList.Items[index]).Content.ToString());

            Profile = selected;
            LblTitle.Content = "Overview of " + Profile.ProfileName;

            //If the previous selected option was a blank option (index -1 or 0)
            //Lock the profile selection, otherwise leave it unlocked
            if (DDLProfileList.Text == "")
            {
                UpdateProfileLock(true);
            }
        }

        private void UpdateProfileLock(bool locked)
        {
            if (locked)
            {
                ImgLockButton.Source = new BitmapImage(new Uri("/img/locked-padlock-shaded.png", UriKind.Relative));
                ImgLockButton.Tag = "Locked";
                ImgLockButton.ToolTip = "Unlock profile selection";
                DDLProfileList.IsEnabled = false;
            }
            else
            {
                ImgLockButton.Source = new BitmapImage(new Uri("/img/locked-padlock.png", UriKind.Relative));
                ImgLockButton.Tag = "Unlocked";
                ImgLockButton.ToolTip = "Lock profile selection";
                DDLProfileList.IsEnabled = true;
            }
        }

        private void BtnLockProfile_Click(object sender, RoutedEventArgs e)
        {
            if (ImgLockButton.Tag.ToString() == "Unlocked")
            {
                UpdateProfileLock(true);
            }
            else
            {
                UpdateProfileLock(false);
            }
        }
    }
}
