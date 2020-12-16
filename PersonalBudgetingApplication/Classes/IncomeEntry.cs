using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class IncomeEntry
    {

        private double _amount;

        public int ProfileId { get; set; }

        public double Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (value < 0) { value *= -1; }
                _amount = value;
            }
        }

        public IncomeType Type { get; set; }

        public DateTime Date { get; set; }

        public int IncomeID { get; set; }

        public IncomeEntry() { }

        public IncomeEntry(double amount)
        {
            Amount = amount;
        }

        public IncomeEntry(double amount, string type, string date)
        {
            Amount = amount;
            Date = DateTime.Parse(date);
            for (int i = 0; i < (int)IncomeType.Other; i++)
            {
                if (type == ((IncomeType)i).ToString())
                {
                    Type = (IncomeType)i;
                }
            }
        }

        public bool SubmitIncomeEntry()
        {
            using (var conn = Common.CreateConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO tblIncome (ProfileID, Inc_Amount, Inc_Type) VALUES (@ProfileID, @Amount, @Type)";
                    cmd.Parameters.AddWithValue("@ProfileID", ProfileId);
                    cmd.Parameters.AddWithValue("@Amount", Amount);
                    cmd.Parameters.AddWithValue("@Type", (int)Type);

                    if (conn.State == System.Data.ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return true;
        }
    }

    public enum IncomeType : int
    {
        Paycheck = 1,
        Gift,
        Refund,
        Other
    }
}
