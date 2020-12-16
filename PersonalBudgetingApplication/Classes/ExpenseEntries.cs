using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class ExpenseEntries : List<ExpenseEntry>
    {
        public ExpenseEntries() { }

        public ExpenseEntries(Profile profile)
        {
            GatherExpenseEntries(profile.ProfileID);
        }

        private void GatherExpenseEntries(int profileId)
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ExpenseId, Exp_Amount, Exp_Type, Exp_Date FROM tblExpense WHERE ProfileId = @ProfileId";
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
