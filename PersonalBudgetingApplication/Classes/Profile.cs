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
