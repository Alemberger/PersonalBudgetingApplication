using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class DebtInterest
    {
        public int ID { get; set; } = -1;

        public int DebtID { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public string RecordBy { get; set; }

        public DateTime RecordDate { get; set; }

        public DebtInterest() { }

        public DebtInterest(int interestId)
        {
            GethInterestRecord(interestId);
        }

        private void GethInterestRecord(int interestId)
        {
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT DebtID, Int_Amount, Int_Date, RecordBy, RecordDate FROM tblDebtInterests WHERE InterestID = @InterestID";
                    cmd.Parameters.Add("@InterestID", DbType.Int32).Value = interestId;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        ID = interestId;
                        DebtID = read.GetInt32(0);
                        Amount = read.GetDouble(1);
                        Date = DateTime.Parse(read.GetString(2));
                        RecordBy = read.GetString(3);
                        RecordDate = DateTime.Parse(read.GetString(4));
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        public bool SubmitInterestRecord()
        {
            //Validation
            try
            {
                //Checks that all necessary object properties are set and exit with failure if they are not set or are set to invalid values
                if (Amount < 0.00) { return false; }
                if (DebtID < 1) { return false; }
                if (ID < -1) { return false; }
                Date.ToString("MM/dd/yyyy");
            }
            catch (NullReferenceException) { return false; }

            //Submission
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    if (ID < 1)
                    {
                        cmd.CommandText = "INSERT INTO tblDebtInterests (DebtID, Int_Amount, Int_Date, RecordBy, RecordDate) VALUES (@DebtID, @Amount, @Date, @RecordBy, @RecordDate)";
                        cmd.Parameters.Add("@DebtID", DbType.Int32).Value = DebtID;
                        cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                        cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                        cmd.Parameters.Add("@RecordDate", DbType.String).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE tblDebtInterests SET Int_Amount = @Amount, Int_Date = @Date, RecordBy = @RecordBy, RecordDate = @RecordDate WHERE InterestID = @InterestID";
                        cmd.Parameters.Add("@InterestID", DbType.Int32).Value = ID;
                        cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                        cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                        cmd.Parameters.Add("@RecordDate", DbType.String).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            //Return Values
            return true;
        }
    }
}
