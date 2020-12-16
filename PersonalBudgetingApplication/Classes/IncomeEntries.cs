using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class IncomeEntries : List<IncomeEntry>
    {
        public IncomeEntries() { }

        public IncomeEntries(Profile profile)
        {
            GatherIncomeEntries(profile.ProfileID);
        }

        private void GatherIncomeEntries(int profileId)
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT IncomeId, Inc_Amount, Inc_Type, Inc_Date FROM tblIncome WHERE ProfileId = @ProfileId";
                    cmd.Parameters.AddWithValue("@ProfileId", profileId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var entry = new IncomeEntry();

                        entry.ProfileId = profileId;
                        entry.IncomeID = read.GetInt32(0);
                        entry.Amount = read.GetDouble(1);
                        entry.Type = (IncomeType)read.GetInt32(2);
                        entry.Date = DateTime.Parse(read.GetString(3));

                        Add(entry);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }
    }
}
