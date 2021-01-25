using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class DebtPayment
    {
        public int ID { get; set; } = -1;

        public int DebtID { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public string RecordBy { get; set; }

        public DateTime RecordDate { get; set; }

        public DebtPayment() { }

        public DebtPayment(int paymentId)
        {
            GetDebtPayment(paymentId);
        }

        public DebtPayment Transfer()
        {
            return new DebtPayment(ID)
            {
                DebtID = DebtID,
                Amount = Amount,
                Date = Date,
                RecordBy = RecordBy,
                RecordDate = RecordDate
            };
        }

        private void GetDebtPayment(int paymentId)
        {
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT DebtID, Pmt_Amount, Pmt_Date, RecordBy, RecordDate FROM tblDebtPayments WHERE PaymentID = @PaymentID";
                    cmd.Parameters.Add("@PaymentID", DbType.Int32).Value = paymentId;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        ID = paymentId;
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

        public void SubmitDebtPayment()
        {
            var valid = true;

            try
            {
                if (Amount < 0.00) { valid = false; }
                if (DebtID < 1) { valid = false; }
                if (ID < -1) { valid = false; }
            }
            catch (NullReferenceException) { valid = false; }

            if (!valid) { throw new DatabaseException("Invalid submission object"); }

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    if (ID < 1)
                    {
                        cmd.CommandText = "INSERT INTO tblDebtPayments (DebtID, Pmt_Amount, Pmt_Date, RecordBy, RecordDate) VALUES (@DebtID, @Amount, @Date, @RecordBy, @RecordDate)";
                        cmd.Parameters.Add("@DebtID", DbType.Int32).Value = DebtID;
                        cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                        cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                        cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE tblDebtPayments SET Pmt_Amount = @Amount, Pmt_Date = @Date, RecordBy = @RecordBy, RecordDate = @RecordDate WHERE PaymentID = @PaymentID";
                        cmd.Parameters.Add("@PaymentID", DbType.Int32).Value = ID;
                        cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                        cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                        cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");
                    }

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }

                var updateCmd = conn.CreateCommand();
                try
                {
                    updateCmd.CommandText = "UPDATE tblDebts SET Dbt_LastUpdateDate = @LastUpdateDate, RecordBy = @RecordBy, RecordDate = @RecordDate WHERE DebtID = @DebtID";
                    updateCmd.Parameters.Add("@DebtID", DbType.Int32).Value = DebtID;
                    updateCmd.Parameters.Add("@LastUpdateDate", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                    updateCmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                    updateCmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    updateCmd.ExecuteNonQuery();
                }
                finally { conn.Close(); updateCmd.Dispose(); }
            }
        }
    }
}
