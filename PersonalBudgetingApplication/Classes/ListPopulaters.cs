using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SqlQueriesForFramework;

namespace PersonalBudgetingApplication.Classes
{
    class ListPopulaters : DataAccess
    {
        public ListPopulaters() { }

        public void PopulateProfileList(ComboBox target)
        {
            List<ComboBoxItem> bindList = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            string selectQuery = "SELECT ProfileID, ProfileName FROM tblProfile";
            List<DataColumn> selectColumns = new List<DataColumn> { new DataColumn("ProfileID", typeof(int)), new DataColumn("ProfileName", typeof(string)) };

            DataTable results = ExecuteSelectQuery(selectQuery, selectColumns);

            foreach (DataRow row in results.Rows)
            {
                ComboBoxItem newItem = new ComboBoxItem() { Content = row.ItemArray[1].ToString(), Tag = row.ItemArray[0].ToString() };
                bindList.Add(newItem);
            }

            target.ItemsSource = bindList;
        }

        public void PopulateIncomeTypeList(ComboBox target)
        {
            string queryString = "SELECT LookupTypeID FROM tblLookupType WHERE TypeName = 'IncomeCategory'";
            List<DataColumn> queryColumns = new List<DataColumn> { new DataColumn("LookupTypeID", typeof(int)) };

            var result = ExecuteSelectQuery(queryString, queryColumns);

            if (result.Rows.Count != 1) { throw new DataException("Unexpected result when getting the LookupTypeID"); }

            int typeId = (int)result.Rows[0].ItemArray[0];

            string incomeCategory = string.Format("SELECT LookupID, Description FROM tblLookup WHERE TypeID = {0}", typeId.ToString());
            List<DataColumn> incomeCats = new List<DataColumn> { new DataColumn("LookupID", typeof(int)), new DataColumn("Description", typeof(string)) };

            var bindResults = ExecuteSelectQuery(incomeCategory, incomeCats);

            var bindList = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            foreach (DataRow row in bindResults.Rows)
            {
                var newItem = new ComboBoxItem() { Content = row[1].ToString(), Tag = row[0].ToString() };
                bindList.Add(newItem);
            }

            target.ItemsSource = bindList;
        }

        public void PopulateExpenseTypeList(ComboBox target)
        {
            string queryString = "SELECT LookupTypeID FROM tblLookupType WHERE TypeName = 'ExpenseCategory'";
            List<DataColumn> queryColumns = new List<DataColumn> { new DataColumn("LookupTypeID", typeof(int)) };

            var result = ExecuteSelectQuery(queryString, queryColumns);

            if (result.Rows.Count != 1) { throw new DataException("Unexpected result when getting the LookupTypeID"); }

            int typeId = (int)result.Rows[0].ItemArray[0];

            string incomeCategory = string.Format("SELECT LookupID, Description FROM tblLookup WHERE TypeID = {0}", typeId.ToString());
            List<DataColumn> incomeCats = new List<DataColumn> { new DataColumn("LookupID", typeof(int)), new DataColumn("Description", typeof(string)) };

            var bindResults = ExecuteSelectQuery(incomeCategory, incomeCats);

            var bindList = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            foreach (DataRow row in bindResults.Rows)
            {
                var newItem = new ComboBoxItem() { Content = row[1].ToString(), Tag = row[0].ToString() };
                bindList.Add(newItem);
            }

            target.ItemsSource = bindList;
        }

        public void PopulateAccountsList(ComboBox target, int profileId)
        {
            if (profileId < 1) { throw new ArgumentException("Valid ProfileID's must be positive"); }

            var selectQuery = string.Format("SELECT AccountID, Acc_name FROM tblAccount WHERE ProfileID = {0}", profileId.ToString());
            var selectColumns = new List<DataColumn> { new DataColumn("AccountID", typeof(int)), new DataColumn("Acc_name", typeof(string)) };

            var results = ExecuteSelectQuery(selectQuery, selectColumns);

            var bindList = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            foreach (DataRow row in results.Rows)
            {
                var newItem = new ComboBoxItem() { Content = row[1].ToString(), Tag = row[0].ToString() };
                bindList.Add(newItem);
            }

            target.ItemsSource = bindList;
        }
    }
}
