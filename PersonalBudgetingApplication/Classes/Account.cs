using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using PersonalBudgetingApplication.Classes.Sorting;

namespace PersonalBudgetingApplication.Classes
{
    /// <summary>
    /// Represents a record from tblAccounts in the SQLite Database
    /// Contains a property for each of the database fields
    /// </summary>
    public class Account
    {
        public int ID { get; set; } = -1;

        public int ProfileID { get; set; }

        public string Name { get; set; }

        public double Amount { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public string RecordBy { get; set; }

        public DateTime RecordDate { get; set; }

        [XmlIgnore]
        public List<Income> Incomes
        {
            get
            {
                return GetIncomes(ID);
            }
        }

        [XmlIgnore]
        public List<Expense> Expenses
        {
            get
            {
                return GetExpenses(ID);
            }
        }

        public Account()
        {
        }

        public Account(int accountId)
        {
            GetAccountRecord(accountId);
        }

        private void GetAccountRecord(int accountId)
        {
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT AccountID, ProfileID, Acc_Name, Acc_Amount, Acc_LastUpdateDate, RecordBy, RecordDate FROM tblAccounts WHERE AccountID = @AccountID";
                    cmd.Parameters.AddWithValue("@AccountID", accountId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        ID = read.GetInt32(0);
                        ProfileID = read.GetInt32(1);
                        Name = read.GetString(2);
                        Amount = read.GetDouble(3);
                        LastUpdateDate = DateTime.Parse(read.GetString(4));
                        RecordBy = read.GetString(5);
                        RecordDate = DateTime.Parse(read.GetString(6));
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }

        public bool SubmitAccountRecord()
        {
            //Validation
            try
            {
                if (ProfileID < 1) { return false; }
                Amount.ToString();
                if (Name == "") { return false; }
                if (LastUpdateDate < new DateTime(2000, 1, 1)) { return false; }
                if (RecordBy == "") { return false; }
                if (RecordDate < new DateTime(2000, 1, 1)) { return false; }
            }
            catch (NullReferenceException) { return false; }

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO tblAccounts (ProfileID, Acc_Name, Acc_Amount, Acc_LastUpdateDate, RecordBy, RecordDate) VALUES (@ProfileID, @Name, @Amount, @LastUpdateDate, @RecordBy, @RecordDate)";
                    cmd.Parameters.Add("@ProfileID", DbType.Int32).Value = ProfileID;
                    cmd.Parameters.Add("@Name", DbType.String).Value = Name;
                    cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                    cmd.Parameters.Add("@LastUpdateDate", DbType.String).Value = LastUpdateDate.ToString("MM/dd/yyyy");
                    cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                    cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return true;
        }

        public bool UpdateAccountBalance(double change, DateTime updateDate)
        {
            try
            {
                if (ID < 1) { return false; }
                Amount.ToString();
            }
            catch (NullReferenceException) { return false; }

            var newAmount = Amount + change;

            if (updateDate > LastUpdateDate)
            {
                LastUpdateDate = updateDate;
            }

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "UPDATE tblAccounts SET Acc_Amount = @Amount, Acc_LastUpdateDate = @LastUpdateDate, RecordBy = @RecordBy, RecordDate = @RecordDate WHERE AccountID = @AccountID";
                    cmd.Parameters.Add("@Amount", DbType.Double).Value = newAmount;
                    cmd.Parameters.Add("@LastUpdateDate", DbType.String).Value = LastUpdateDate.ToString("MM/dd/yyyy");
                    cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                    cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");
                    cmd.Parameters.Add("@AccountID", DbType.Int32).Value = ID;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return true;
        }

        public DateTime GetEarliestDate()
        {
            var date = LastUpdateDate;

            foreach (Income record in Incomes)
            {
                if (record.Date.Date < date.Date)
                {
                    date = record.Date;
                }
            }

            foreach (Expense record in Expenses)
            {
                if (record.Date.Date < date.Date)
                {
                    date = record.Date;
                }
            }

            return date;
        }

        public int GetDateRangeInDays(DateTime earlyDate, DateTime lateDate)
        {
            return Common.CalculateDifferenceInDays(earlyDate, lateDate);
        }

        public List<Income> GetIncomesforDateList(DateTime date)
        {
            var dateIncomes = new List<Income>();

            foreach (Income item in Incomes)
            {
                if (date.Date == item.Date.Date)
                {
                    dateIncomes.Add(item.Transfer());
                }
            }

            return dateIncomes;
        }

        public List<Expense> GetExpensesforDateList(DateTime date)
        {
            var dateExpenses = new List<Expense>();

            foreach (Expense item in Expenses)
            {
                if (date.Date == item.Date.Date)
                {
                    dateExpenses.Add(item.Transfer());
                }
            }

            return dateExpenses;
        }

        private List<Income> GetIncomes(int accountId)
        {
            var incomes = new List<Income>();

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT IncomeID, Inc_Amount, Inc_Type, Inc_Date, RecordBy, RecordDate FROM tblIncomes WHERE AccountID = @AccountID";
                    cmd.Parameters.AddWithValue("@AccountID", accountId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var income = new Income() { ID = read.GetInt32(0), AccountID = accountId, Amount = read.GetDouble(1), Type = (IncomeType)read.GetInt32(2), Date = DateTime.Parse(read.GetString(3)), RecordBy = read.GetString(4), RecordDate = DateTime.Parse(read.GetString(5)) };

                        incomes.Add(income);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            var sorter = new IncomeSorter(incomes);

            incomes = sorter.ByDateDescending();

            return incomes;
        }

        private List<Expense> GetExpenses(int accountId)
        {
            var expenses = new List<Expense>();

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT ExpenseID, Exp_Amount, Exp_Type, Exp_Date, RecordBy, RecordDate FROM tblExpenses WHERE AccountID = @AccountID";
                    cmd.Parameters.AddWithValue("@AccountID", accountId);

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        var expense = new Expense() { ID = read.GetInt32(0), AccountID = accountId, Amount = read.GetDouble(1), Type = (ExpenseType)read.GetInt32(2), Date = DateTime.Parse(read.GetString(3)), RecordBy = read.GetString(4), RecordDate = DateTime.Parse(read.GetString(5)) };

                        expenses.Add(expense);
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return expenses;
        }

        public bool SubmitAccount()
        {
            try
            {
                if (ProfileID < 1) { return false; }
                if (Name == "") { return false; }
                if (Amount < 0.00) { return false; }
                if (LastUpdateDate < DateTime.Parse("2000/01/01")) { return false; }
                if (RecordBy == "") { return false; }
                if (RecordDate < DateTime.Parse("2000/01/01")) { return false; }
            }
            catch (NullReferenceException) { return false; }

            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "INSERT INTO tblAccounts (ProfileID, Acc_Name, Acc_Amount, Acc_LastUpdateDate, RecordBy, RecordDate) VALUES (@ProfileID, @Name, @Amount, @LastUpdateDate, @RecordBy, @RecordDate)";
                    cmd.Parameters.Add("@ProfileID", DbType.Int32).Value = ProfileID;
                    cmd.Parameters.Add("@Name", DbType.String).Value = Name;
                    cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                    cmd.Parameters.Add("@LastUpdateDate", DbType.String).Value = LastUpdateDate.ToString("MM/dd/yyyy");
                    cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                    cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("MM/dd/yyyy HH:mm");

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return true;
        }
    }
}
