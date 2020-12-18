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

namespace PersonalBudgetingApplication.Classes
{
    /// <summary>
    /// Represents a record from tblAccounts in the SQLite Database
    /// Contains a property for each of the database fields
    /// </summary>
    public class Account
    {
        public int ID { get; set; }

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
                    cmd.CommandText = "SELECT AccoutID, ProfileID, Acc_Name, Acc_Amount, Acc_LastUpdateDate, RecordBy, RecordDate FROM tblAccounts WHERE AccountID = @AccountID";
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
                        RecordDate = DateTime.Parse(read.GetString(5));
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
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
    }
}
