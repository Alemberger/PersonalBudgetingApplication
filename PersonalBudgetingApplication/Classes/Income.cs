using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class Income
    {
        public int ID { get; set; } = -1;

        public int AccountID { get; set; }

        public double Amount { get; set; }

        public IncomeType Type { get; set; }

        public DateTime Date { get; set; }

        public string RecordBy { get; set; }

        public DateTime RecordDate { get; set; }

        public Income() { }

        public Income(int incomeId)
        {
            GatherIncomeRecord(incomeId);
        }

        private void GatherIncomeRecord(int incomeId)
        {
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT AccountID, Inc_Amount, Inc_Type, Inc_Date, RecordBy, RecordDate FROM tblIncomes WHERE IncomeID = @IncomeID";
                    cmd.Parameters.Add("@IncomeID", DbType.Int32).Value = incomeId;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        ID = incomeId;
                        AccountID = read.GetInt32(0);
                        Amount = read.GetDouble(1);
                        Type = (IncomeType)read.GetInt32(2);
                        Date = DateTime.Parse(read.GetString(3));
                        RecordBy = read.GetString(4);
                        RecordDate = DateTime.Parse(read.GetString(5));
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }
    }

    public enum IncomeType
    {
        Paycheck = 1,
        Gift = 2,
        Other = 3
    }
}
