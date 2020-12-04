using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PersonalBudgetingApplication.Classes
{
    class Common
    {
        public void PopulateProfileList(ComboBox target)
        {
            //Add the first blank value to the combo box
            target.Items.Add(new ComboBoxItem());
            var conn = new SQLiteConnection("Data Source=C:\\PersonalBudget\\BudgetDB.db;");
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            var cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT ProfileName FROM tblProfile";

            var read = cmd.ExecuteReader();
            while (read.Read())
            {
                target.Items.Add(new ComboBoxItem() { Content = read.GetString(0) });
            }
            conn.Close();
        }
    }
}
