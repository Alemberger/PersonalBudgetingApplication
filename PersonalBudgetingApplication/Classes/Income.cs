using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalBudgetingApplication.Classes.Sorting;

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

        public bool SubmitRecord()
        {
            try
            {
                if (AccountID < 1) { return false; }
                if (Amount < 0.00) { return false; }
                if (Type.ToString() == "0" || Type.ToString() == "") { return false; }
                if (Date < new DateTime(2000, 1, 1)) { return false; }
                if (RecordBy == "") { return false; }
                if (RecordDate < new DateTime(2000, 1, 1)) { return false; }
            }
            catch (NullReferenceException) { return false; }

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO tblIncomes (AccountID, Inc_Amount, Inc_Type, Inc_Date, RecordBy, RecordDate) VALUES (@AccountID, @Amount, @Type, @Date, @RecordBy, @RecordDate)";
                    cmd.Parameters.Add("@AccountID", DbType.Int32).Value = AccountID;
                    cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                    cmd.Parameters.Add("@Type", DbType.Int32).Value = (int)Type;
                    cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                    cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                    cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return true;
        }

        public bool UpdateRecord()
        {
            try
            {
                if (ID < 1) { return false; }
                if (AccountID < 1) { return false; }
                if (Amount < 0.00) { return false; }
                if (Type.ToString() == "0" || Type.ToString() == "") { return false; }
                if (Date < new DateTime(2000, 1, 1)) { return false; }
                if (RecordBy == "") { return false; }
                if (RecordDate < new DateTime(2000, 1, 1)) { return false; }
            }
            catch (NullReferenceException) { return false; }

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "UPDATE tblIncomes SET Inc_Amount = @Amount, Inc_Type = @Type, Inc_Date = @Date, RecordBy = @RecordBy, RecordDate = @RecordDate WHERE IncomeID = @IncomeID";
                    cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                    cmd.Parameters.Add("@Type", DbType.Int32).Value = (int)Type;
                    cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                    cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                    cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");
                    cmd.Parameters.Add("@IncomeID", DbType.Int32).Value = ID;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return true;
        }

        public Income Transfer()
        {
            return new Income()
            {
                ID = ID,
                AccountID = AccountID,
                Amount = Amount,
                Date = Date,
                Type = Type,
                RecordBy = RecordBy,
                RecordDate = RecordDate
            };
        }
    }

    public enum IncomeType
    {
        Paycheck = 1,
        Gift = 2,
        Other = 3
    }
}
