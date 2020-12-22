using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PersonalBudgetingApplication.Classes
{
    /// <summary>
    /// Class representation of the tblDebts database table
    /// </summary>
    public class Debt
    {
        public int ID { get; set; } = -1;

        public int ProfileID { get; set; }

        public string Name { get; set; }

        public double Principal { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public InterestType InterestType { get; set; }

        public CompoundNumberApplied TimesApplied { get; set; }

        public double AnnualPercentageRate { get; set; }

        public string RecordBy { get; set; }

        public DateTime RecordDate { get; set; }

        [XmlIgnore]
        public List<DebtPayment> Payments
        {
            get
            {
                return GetDebtPayments(ID);
            }
        }

        [XmlIgnore]
        public List<DebtIncrease> DebtInterests
        {
            get
            {
                return GetDebtInterests(ID);
            }
        }

        public Debt() { }

        private List<DebtPayment> GetDebtPayments(int debtId)
        {
            var payments = new List<DebtPayment>();

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT PaymentID, Pmt_Amount, Pmt_Date, RecordBy, RecordDate FROM tblDebtPayments WHERE DebtID = @DebtID";
                    cmd.Parameters.AddWithValue("@DebtID", debtId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var payment = new DebtPayment() { ID = read.GetInt32(0), DebtID = debtId, Amount = read.GetDouble(1), Date = DateTime.Parse(read.GetString(2)), RecordBy = read.GetString(3), RecordDate = DateTime.Parse(read.GetString(4)) };

                        payments.Add(payment);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return payments;
        }

        private List<DebtIncrease> GetDebtInterests(int debtId)
        {
            var debtInterests = new List<DebtIncrease>();

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT InterestID, Inc_Amount, Inc_Date, Inc_Type, recordBy, RecordDate FROM tblDebtIncreases WHERE DebtID = @DebtID";
                    cmd.Parameters.AddWithValue("@DebtID", debtId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var interest = new DebtIncrease() { ID = read.GetInt32(0), DebtID = debtId, Amount = read.GetDouble(1), Date = DateTime.Parse(read.GetString(2)), RecordBy = read.GetString(3), RecordDate = DateTime.Parse(read.GetString(4)) };

                        debtInterests.Add(interest);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return debtInterests;
        }
    }

    public enum InterestType
    {
        Unspecified = 0,
        Static,
        Compound
    }

    public enum CompoundNumberApplied
    {
        None = 0,
        Annually = 1,
        Semiannually = 2,
        Quarterly = 4,
        Monthly = 12,
        Weekly = 52,
        Daily = 365
    }
}
