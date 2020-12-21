using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class Expense
    {
        public int ID { get; set; } = -1;

        public int AccountID { get; set; }

        public double Amount { get; set; }

        public ExpenseType Type { get; set; }

        public DateTime Date { get; set; }

        public string RecordBy { get; set; }

        public DateTime RecordDate { get; set; }

        public Expense() { }

        public Expense(int ExpenseId)
        {
            GatherExpenseRecord(ExpenseId);
        }

        private void GatherExpenseRecord(int expenseId)
        {
            using (var conn = DataAccess.EstablishConnection())
            {
                var cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "SELECT AccountID, Exp_Amount, Exp_Type, Exp_Date, RecordBy, RecordDate FROM tblExpenses WHERE ExpenseID = @ExpenseID";
                    cmd.Parameters.Add("@ExpenseID", DbType.Int32).Value = expenseId;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    var read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        ID = expenseId;
                        AccountID = read.GetInt32(0);
                        Amount = read.GetDouble(1);
                        Type = (ExpenseType)read.GetInt32(2);
                        Date = DateTime.Parse(read.GetString(3));
                        RecordBy = read.GetString(4);
                        RecordDate = DateTime.Parse(read.GetString(5));
                    }
                    read.Close();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }
        }
    }

    public enum ExpenseType
    {
        Rent = 1,
        Food = 2,
        Vehicle = 3,
        Entertainment = 4,
        Utility = 5,
        Other = 6
    }
}
