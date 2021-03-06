﻿using System;
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
            GetExpenseRecord(ExpenseId);
        }

        private void GetExpenseRecord(int expenseId)
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

        public bool SubmitRecord()
        {
            try
            {
                if (AccountID < 1) { return false; }
                if (Amount < 0.00) { return false; }
                if ((int)Type < 0) { return false; }
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
                    cmd.CommandText = "INSERT INTO tblExpenses (AccountID, Exp_Amount, Exp_Date, Exp_Type, RecordBy, RecordDate) VALUES (@AccountID, @Amount, @Date, @Type, @RecordBy, @RecordDate)";
                    cmd.Parameters.Add("@AccountID", DbType.Int32).Value = AccountID;
                    cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                    cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                    cmd.Parameters.Add("@Type", DbType.Int32).Value = (int)Type;
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
                if ((int)Type < 0) { return false; }
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
                    cmd.CommandText = "UPDATE tblExpenses SET Amount = @Amount, Date = @Date, Type = @Type, RecordBy = @RecordBy, RecordDate = @RecordDate WHERE ExpenseID = @ExpenseID";
                    cmd.Parameters.Add("@Amount", DbType.Double).Value = Amount;
                    cmd.Parameters.Add("@Date", DbType.String).Value = Date.ToString("MM/dd/yyyy");
                    cmd.Parameters.Add("@Type", DbType.Int32).Value = (int)Type;
                    cmd.Parameters.Add("@RecordBy", DbType.String).Value = RecordBy;
                    cmd.Parameters.Add("@RecordDate", DbType.String).Value = RecordDate.ToString("yyyy-MM-dd HH:mm");
                    cmd.Parameters.Add("@ExpenseID", DbType.Int32).Value = ID;

                    if (conn.State == ConnectionState.Closed) { conn.Open(); }

                    cmd.ExecuteNonQuery();
                }
                finally { conn.Close(); cmd.Dispose(); }
            }

            return true;
        }

        public Expense Transfer()
        {
            return new Expense()
            {
                ID = ID,
                AccountID = AccountID,
                Amount = Amount,
                Type = Type,
                Date = Date,
                RecordBy = RecordBy,
                RecordDate = RecordDate
            };
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
