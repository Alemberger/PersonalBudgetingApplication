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
    /// Interaction logic for DebtEntryWindow.xaml
    /// </summary>
    public partial class DebtEntryWindow : Window
    {
        public Profile Profile { get; set; }

        public DebtEntryWindow()
        {
            InitializeComponent();
        }

        public DebtEntryWindow(Profile profile)
        {
            InitializeComponent();

            Profile = profile;
        }
    }
}
