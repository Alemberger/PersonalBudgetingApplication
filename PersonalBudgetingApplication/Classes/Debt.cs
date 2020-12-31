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
                return GetDebtPayments();
            }
        }

        [XmlIgnore]
        public List<DebtIncrease> DebtIncreases
        {
            get
            {
                return GetDebtIncreases();
            }
        }

        public Debt() { }

        public Debt(Profile profile)
        {
            ProfileID = profile.ProfileID;
        }

        public Debt (int debtID)
        {
            GetDebtRecord(debtID);
        }

        private void GetDebtRecord(int debtID)
        {
            ID = debtID;

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ProfileID, Dbt_Name, Dbt_Principal, Dbt_LastUpdateDate, Dbt_InterestType, Dbt_NumberOfTimesApplied, Dbt_AnnualPercentageRate, RecordBy, RecordDate FROM tblDebts WHERE DebtID = @DebtID";
                    cmd.Parameters.Add("@DebtID", DbType.Int32).Value = debtID;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        ProfileID = read.GetInt32(0);
                        Name = read.GetString(1);
                        Principal = read.GetDouble(2);
                        LastUpdateDate = DateTime.Parse(read.GetString(3));
                        InterestType = (InterestType)read.GetInt32(4);
                        TimesApplied = (CompoundNumberApplied)read.GetInt32(5);
                        AnnualPercentageRate = read.GetDouble(6);
                        RecordBy = read.GetString(7);
                        RecordDate = DateTime.Parse(read.GetString(8));
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        public DateTime GetEarliestDate()
        {
            var date = LastUpdateDate;

            foreach (DebtIncrease item in DebtIncreases)
            {
                if (item.Date.Date < date.Date)
                {
                    date = item.Date;
                }
            }

            foreach (DebtPayment item in Payments)
            {
                if (item.Date.Date < date.Date)
                {
                    date = item.Date;
                }
            }

            return date;
        }

        public List<DebtPayment> GetDebtPaymentsForDate(DateTime date)
        {
            var datePayments = new List<DebtPayment>();

            foreach (DebtPayment item in Payments)
            {
                if (item.Date.Date == date.Date)
                {
                    datePayments.Add(item.Transfer());
                }
            }

            return datePayments;
        }

        public List<DebtIncrease> GetDebtIncreasesForDate(DateTime date)
        {
            var dateIncreases = new List<DebtIncrease>();

            foreach (DebtIncrease item in DebtIncreases)
            {
                if (item.Date.Date == date.Date)
                {
                    dateIncreases.Add(item.Transfer());
                }
            }

            return dateIncreases;
        }

        public int CalculateDateRange()
        {
            return Common.CalculateDifferenceInDays(GetEarliestDate(), LastUpdateDate);
        }

        private List<DebtPayment> GetDebtPayments()
        {
            var payments = new List<DebtPayment>();

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT PaymentID, Pmt_Amount, Pmt_Date, RecordBy, RecordDate FROM tblDebtPayments WHERE DebtID = @DebtID";
                    cmd.Parameters.AddWithValue("@DebtID", ID);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var payment = new DebtPayment() { ID = read.GetInt32(0), DebtID = ID, Amount = read.GetDouble(1), Date = DateTime.Parse(read.GetString(2)), RecordBy = read.GetString(3), RecordDate = DateTime.Parse(read.GetString(4)) };

                        payments.Add(payment);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return payments;
        }

        private List<DebtIncrease> GetDebtIncreases()
        {
            var debtInterests = new List<DebtIncrease>();

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT IncreaseID, Inc_Amount, Inc_Date, Inc_Type, recordBy, RecordDate FROM tblDebtIncreases WHERE DebtID = @DebtID";
                    cmd.Parameters.AddWithValue("@DebtID", ID);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var interest = new DebtIncrease() { ID = read.GetInt32(0), DebtID = ID, Amount = read.GetDouble(1), Date = DateTime.Parse(read.GetString(2)), RecordBy = read.GetString(3), RecordDate = DateTime.Parse(read.GetString(4)) };

                        debtInterests.Add(interest);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return debtInterests;
        }

        public void SubmitDebt()
        {
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    if (ID == -1)
                    {
                        cmd.CommandText = "INSERT INTO tblDebts (ProfileID, Dbt_Name, Dbt_Principal, Dbt_LastUpdateDate, Dbt_InterestType, Dbt_NumberOfTimesApplied, Dbt_AnnualPercentageRate, RecordBy, RecordDate) VALUES (@ProfileID, @Name, @Principal, @Date, @InterestType, @InterestFrequency, @APR, @RecordBy, @RecordDate)";
                        cmd.Parameters.Add("@ProfileID", DbType.Int32).Value = ProfileID;
                        cmd.Parameters.Add("@Name", DbType.String).Value = Name;
                        cmd.Parameters.Add("@Principal", DbType.Double).Value = Principal;
                        cmd.Parameters.Add("@Date", DbType.String).Value = LastUpdateDate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@InterestType", DbType.Int32).Value = (int)InterestType;
                        cmd.Parameters.Add("@InterestFrequency", DbType.Int32).Value = (int)TimesApplied;
                        cmd.Parameters.Add("@APR", DbType.Double).Value = AnnualPercentageRate;
                        cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                        cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE tblDebts SET Dbt_Name = @Name, Dbt_Principal = @Principal, Dbt_LastUpdateDate = @Date, Dbt_InterestType = @InterestType, Dbt_NumberOfTimesApplied = @InterestFrequency, Dbt_AnnualPercentageRate = @APR, RecordBy = @RecordBy, RecordDate = @RecordDate WHERE DebtID = @DebtID";
                        cmd.Parameters.Add("@DebtID", DbType.Int32).Value = ID;
                        cmd.Parameters.Add("@Name", DbType.String).Value = Name;
                        cmd.Parameters.Add("@Principal", DbType.Double).Value = Principal;
                        cmd.Parameters.Add("@Date", DbType.String).Value = LastUpdateDate.ToString("MM/dd/yyyy");
                        cmd.Parameters.Add("@InterestType", DbType.Int32).Value = (int)InterestType;
                        cmd.Parameters.Add("@InterestFrequency", DbType.Int32).Value = (int)TimesApplied;
                        cmd.Parameters.Add("@APR", DbType.Double).Value = AnnualPercentageRate;
                        cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                        cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");
                    }

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }
    }

    public enum InterestType
    {
        Unspecified = 0,
        Charge,
        StaticInterest,
        CompoundInterest
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
