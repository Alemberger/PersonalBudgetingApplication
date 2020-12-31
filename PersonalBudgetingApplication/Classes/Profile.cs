using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace PersonalBudgetingApplication.Classes
{
    /// <summary>
    /// This class contains methods and properties that relate to a user Profile as stored in the database schema
    /// </summary>
    public class Profile
    {
        public int ProfileID { get; set; } = -1;

        [XmlAttribute("Name")]
        public string ProfileName { get; set; }

        [XmlIgnore]
        public List<Account> Accounts
        {
            get
            {
                return GetAccounts(ProfileID);
            }
        }
        
        [XmlIgnore]
        public List<Debt> Debts
        {
            get
            {
                return GetDebts(ProfileID);
            }
        }

        public Profile() { }

        public Profile(int profileID)
        {
            ProfileID = profileID;
            ProfileName = GetProfileName(profileID);
        }

        public Profile(string profileName)
        {
            ProfileName = profileName;
            ProfileID = GetProfileID(profileName);
        }

        private int GetProfileID(string profileName)
        {
            int id = -1;
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ProfileID FROM tblProfile WHERE ProfileName = @ProfileName";
                    cmd.Parameters.AddWithValue("@ProfileName", profileName);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        id = read.GetInt32(0);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return id;
        }

        private string GetProfileName(int profileID)
        {
            string name = "";
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ProfileName FROM tblProfile WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileID);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        name = read.GetString(0);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return name;
        }

        private List<Account> GetAccounts(int profileId)
        {
            var accounts = new List<Account>();

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT AccountID, Acc_Name, Acc_Amount, Acc_LastUpdateDate, RecordBy, RecordDate FROM tblAccounts WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var account = new Account() { ID = read.GetInt32(0), ProfileID = profileId, Name = read.GetString(1), Amount = read.GetDouble(2), LastUpdateDate = DateTime.Parse(read.GetString(3)), RecordBy = read.GetString(4), RecordDate = DateTime.Parse(read.GetString(5)) };

                        accounts.Add(account);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return accounts;
        }

        private List<Debt> GetDebts(int profileId)
        {
            var debts = new List<Debt>();

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT DebtID, Dbt_Name, Dbt_Principal, Dbt_LastUpdateDate, Dbt_InterestType, Dbt_NumberOfTimesApplied, Dbt_AnnualPercentageRate, RecordBy, RecordDate FROM tblDebts WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var debt = new Debt() { ID = read.GetInt32(0), ProfileID = profileId, Name = read.GetString(1), Principal = read.GetDouble(2), LastUpdateDate = DateTime.Parse(read.GetString(3)), InterestType = (InterestType)read.GetInt32(4), TimesApplied = (CompoundNumberApplied)read.GetInt32(5), AnnualPercentageRate = read.GetDouble(6), RecordBy = read.GetString(7), RecordDate = DateTime.Parse(read.GetString(8)) };

                        debts.Add(debt);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return debts;
        }

        public List<ComboBoxItem> ListAccounts()
        {
            var items = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            List<Account> accounts = new List<Account>();
            try
            {
                accounts = Accounts;
            }
            catch (NullReferenceException) { return items; }

            for (int i = 0; i < accounts.Count; i++)
            {
                var account = accounts[i];

                items.Add(new ComboBoxItem() { Content = account.Name, Tag = account.ID.ToString() });
            }

            return items;
        }

        public List<ComboBoxItem> ListDebts()
        {
            var items = new List<ComboBoxItem> { new ComboBoxItem() { Content = "", Tag = "" } };

            List<Debt> debts = new List<Debt>();
            try
            {
                debts = Debts;
            }
            catch (NullReferenceException) { return items; }

            for (int i = 0; i < debts.Count; i++)
            {
                var debt = debts[i];

                items.Add(new ComboBoxItem() { Content = debt.Name, Tag = debt.ID.ToString() });
            }

            return items;
        }
    }
}
