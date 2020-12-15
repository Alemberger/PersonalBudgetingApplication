using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PersonalBudgetingApplication.Classes
{
    /// <summary>
    /// This class contains methods and properties that relate to a user Profile as stored in the database schema
    /// </summary>
    public class Profile
    {

        public int ProfileID { get; set; }

        [XmlAttribute("Name")]
        public string ProfileName { get; set; }

        
        public List<IncomeEntry> IncomeEntries
        {
            get
            {
                return GetIncomeEntries(ProfileID);
            }
        }

        
        public List<ExpenseEntry> ExpenseEntries
        {
            get { return GetExpenseEntries(ProfileID); }
        }

        public List<SavingsEntry> SavingsEntries
        {
            get
            {
                return GetSavingsEntries(ProfileID);
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

        private List<IncomeEntry> GetIncomeEntries(int ProfileId)
        {
            var Entries = new List<IncomeEntry>();

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT IncomeID, Inc_Amount, Inc_Type, Inc_Date FROM tblIncome WHERE ProfileID = @ProfileId";
                    cmd.Parameters.AddWithValue("@ProfileId", ProfileId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new IncomeEntry();

                        entry.IncomeID = read.GetInt32(0);
                        entry.ProfileId = ProfileId;
                        entry.Amount = read.GetDouble(1);
                        entry.Type = (IncomeType)read.GetInt32(2);
                        entry.Date = read.GetString(3);

                        Entries.Add(entry);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return Entries;
        }

        private List<ExpenseEntry> GetExpenseEntries(int profileId)
        {
            var entries = new List<ExpenseEntry>();

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ExpenseID, Exp_Amount, Exp_Type, Exp_Date FROM tblExpense WHERE ProfileID = @ProfileId";
                    cmd.Parameters.AddWithValue("@ProfileId", profileId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new ExpenseEntry();

                        entry.ProfileId = profileId;
                        entry.ExpenseId = read.GetInt32(0);
                        entry.Amount = read.GetDouble(1);
                        entry.Type = (ExpenseType)read.GetInt32(2);
                        entry.Date = read.GetString(3);

                        entries.Add(entry);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return entries;
        }

        private List<SavingsEntry> GetSavingsEntries(int ProfileId)
        {
            var entries = new List<SavingsEntry>();

            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT SavingsID, Sav_Amount, Sav_Date FROM tblSavings WHERE ProfileID = @ProfileId";
                    cmd.Parameters.AddWithValue("@ProfileId", ProfileId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new SavingsEntry();

                        entry.ProfileId = ProfileId;
                        entry.SavingsId = read.GetInt32(0);
                        entry.Amount = read.GetDouble(1);
                        entry.Date = read.GetString(2);

                        entries.Add(entry);
                    }

                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return entries;
        }

        private int GetProfileID(string profileName)
        {
            int id = -1;
            using (var conn = Common.CreateConnection())
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
            using (var conn = Common.CreateConnection())
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
    }
}
