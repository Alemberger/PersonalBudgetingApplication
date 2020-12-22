using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class DebtIncrease
    {
        public int ID { get; set; } = -1;

        public int DebtID { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public DebtIncreaseType IncreaseType { get; set; }

        public string RecordBy { get; set; }

        public DateTime RecordDate { get; set; }

        public DebtIncrease() { }

        public DebtIncrease(int interestId)
        {
            GethIncreaseRecord(interestId);
        }

        private void GethIncreaseRecord(int increaseId)
        {
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT DebtID, Inc_Amount, Inc_Date, Inc_Type, RecordBy, RecordDate FROM tblDebtInterests WHERE IncreaseID = @IncreaseID";
                    cmd.Parameters.Add("@IncreaseID", DbType.Int32).Value = increaseId;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        ID = increaseId;
                        DebtID = read.GetInt32(0);
                        Amount = read.GetDouble(1);
                        Date = DateTime.Parse(read.GetString(2));
                        IncreaseType = (DebtIncreaseType)read.GetInt32(3);
                        RecordBy = read.GetString(4);
                        RecordDate = DateTime.Parse(read.GetString(5));
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
                if ((int)IncreaseType < 0) { return false; }
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
                        cmd.CommandText = "INSERT INTO tblDebtIncreases (DebtID, Inc_Amount, Inc_Date, Inc_Type, RecordBy, RecordDate) VALUES (@DebtID, @Amount, @Date, @Type, @RecordBy, @RecordDate)";
                        cmd.Parameters.Add("@DebtID", DbType.Int32).Value = DebtID;
                        cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                        cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@Type", DbType.Int32).Value = (int)IncreaseType;
                        cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                        cmd.Parameters.Add("@RecordDate", DbType.String).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE tblDebtInterests SET Inc_Amount = @Amount, Inc_Date = @Date, Inc_Type = @Type, RecordBy = @RecordBy, RecordDate = @RecordDate WHERE IncreaseID = @IncreaseID";
                        cmd.Parameters.Add("@IncreaseID", DbType.Int32).Value = ID;
                        cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                        cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@Type", DbType.Int32).Value = (int)IncreaseType;
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

    public enum DebtIncreaseType
    {
        None = 0,
        Interest,
        Charge,
        Other
    }
}
