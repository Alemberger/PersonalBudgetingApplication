using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class SavingsEntries : List<SavingsEntry>
    {
        public SavingsEntries() { }

        public SavingsEntries(Profile profile)
        {
            GatherSavingsEntries(profile.ProfileID);
        }

        private void GatherSavingsEntries(int profileId)
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT SavingsId, Sav_Amount, Sav_Date FROM tblSavings WHERE ProfileID = @ProfileID";
                    cmd.Parameters.AddWithValue("@ProfileID", profileId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new SavingsEntry();

                        entry.ProfileId = profileId;
                        entry.SavingsId = read.GetInt32(0);
                        entry.Amount = read.GetDouble(1);
                        entry.Date = DateTime.Parse(read.GetString(2));

                        Add(entry);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }
    }
}
