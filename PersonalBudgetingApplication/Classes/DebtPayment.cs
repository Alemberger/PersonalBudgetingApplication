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
    }
}
