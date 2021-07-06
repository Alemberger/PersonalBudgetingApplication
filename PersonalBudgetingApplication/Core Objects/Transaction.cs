using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalBudgetingApplication.Classes;

namespace PersonalBudgetingApplication.Core_Objects
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public double? Amount { get; set; }
        public DateTime? Date { get; set; }
        public TransactionType Type { get; set; }
        public int? TypeID { get; set; }
        public int? Category { get; set; }
        public string RecordBy { get; set; }
        public DateTime? RecordDate { get; set; }

        public Account Account { get; set; }

        public EntityState State { get; set; }

        public Transaction()
        {
            TransactionID = -1;
            State = EntityState.Added;
        }

        public Transaction(int id)
        {
            if (id < 1) { throw new ArgumentException("Must provide a valid TransactionID"); }
            //Validate the record exists
            TransactionID = id;
            GetRecord();
        }

        private void GetRecord()
        {
            if (TransactionID < 1) { throw new ArgumentException("Must provide a valid TransactionID to retrieve a record"); }
            try
            {
                var selectQuery = string.Format("SELECT AccountID, Trn_amount, Trn_date, Trn_type, Trn_category, Recordby, RecordDate FROM tblTransaction WHERE TransactionID = {0}", TransactionID.ToString());
                var selectColumns = new List<DataColumn> { new DataColumn("AccountID", typeof(int)), new DataColumn("Amount", typeof(double)), new DataColumn("Type", typeof(int)), new DataColumn("Category", typeof(int)), new DataColumn("RecordBy", typeof(string)), new DataColumn("RecordDate", typeof(DateTime)) };

                var result = DataAccess.ExecuteSelectQuery(selectQuery, selectColumns);

                if (result.Rows.Count != 1) { throw new DataException("Unexpected number of records returned"); }

                Account = new Account((int)result.Rows[0].ItemArray[0]);
                Amount = (double?)result.Rows[0].ItemArray[1];
                Date = (DateTime?)result.Rows[0].ItemArray[2];
                TypeID = (int?)result.Rows[0].ItemArray[3];
                Category = (int?)result.Rows[0].ItemArray[4];
                RecordBy = result.Rows[0].ItemArray[5].ToString();
                RecordDate = (DateTime?)result.Rows[0].ItemArray[6];
            }
            catch (Exception ex) { throw ex; }
        }

        public void SaveChanges()
        {
            //Validate all required values are provided
            if (Amount is null || Amount < 0.00) { throw new ArgumentException("Must provide a transaction Amount"); }
            if (TypeID is null || (TypeID != 1 && TypeID != 2)) { throw new ArgumentException("Must proivde a valid Transaction Type"); }
            if (Account is null) { throw new ArgumentException("Must link an Account to the transaction"); }
            if (Date is null || Date.Value < new DateTime(2020, 1, 1)) { throw new ArgumentException("Must provide a valid date"); }
            //Determine what State the object is in and take the appropriate saving action
            switch (State)
            {
                case EntityState.Added:
                    if (TransactionID > 0) { throw new ArgumentException("Cannot insert a record already containing a TransactionID"); }
                    var id = Insert();
                    TransactionID = id;
                    State = EntityState.Unchanged;
                    break;
                case EntityState.Modified:
                    if (TransactionID < 1) { throw new ArgumentException("Must provide a valid TransactionID to update a record"); }
                    Update();
                    State = EntityState.Unchanged;
                    break;
                case EntityState.Deleted:
                    if (TransactionID < 1) { throw new ArgumentException("Must provide a valid TransactionID to delete a record"); }
                    Delete();
                    State = EntityState.Detached;
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    throw new InvalidOperationException("Unhandled EntityState provided");
            }
        }

        private int Insert()
        {
            var insertQuery = string.Format("INSERT INTO tblTransaction (AccountID, Trn_amount, Trn_date, Trn_Type, Trn_category, RecordBy, RecordDate) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                GetAccountIDForSQL(), GetAmountForSQL(), GetDateForSQL(), GetTypeForSQL(), GetCategoryForSQL(), GetRecordByForSQL(), GetRecordDateForSQL());

            try
            {
                DataAccess.ExecuteNonQuery(insertQuery);
            }
            catch (Exception ex) { throw ex; }

            try
            {
                var selectQuery = "SELECT TransactionID from tblTransaction ORDER BY TransactionID DESC LIMIT 1";
                var selectColumn = new List<DataColumn> { new DataColumn("TransactionID", typeof(int)) };

                var result = DataAccess.ExecuteSelectQuery(selectQuery, selectColumn);

                if (result.Rows.Count != 1) { throw new DataException("An unexpected number of records were returned"); }

                return Convert.ToInt32(result.Rows[0].ItemArray[0]);
            }
            catch (Exception ex) { throw ex; }
        }

        private void Update()
        {
            var updateQuery = string.Format("UPDATE tblTransaction SET AccountID = {0}, Trn_amount = {1}, Trn_date = {2}, Trn_Type = {3}, Trn_category = {4}, RecordBy = {5}, RecordDate = {6} WHERE TransactionID = {7}",
                GetAccountIDForSQL(), GetAmountForSQL(), GetDateForSQL(), GetTypeForSQL(), GetCategoryForSQL(), GetRecordByForSQL(), GetRecordDateForSQL(), GetTransactionIDForSQL());

            DataAccess.ExecuteNonQuery(updateQuery);
        }

        private void Delete()
        {
            var deleteQuery = string.Format("DELETE FROM tblTransaction WHERE TransactionID = {0}", GetTransactionIDForSQL());

            DataAccess.ExecuteNonQuery(deleteQuery);
        }

        private string GetTransactionIDForSQL()
        {
            if (TransactionID < 1)
            {
                return "NULL";
            }
            else
            {
                return TransactionID.ToString();
            }
        }

        private string GetAccountIDForSQL()
        {
            if (Account is null)
            {
                return "NULL";
            }
            else
            {
                if (Account.AccountID < 1) { return "NULL"; }
                else { return Account.AccountID.ToString(); }
            }
        }

        private string GetAmountForSQL()
        {
            if (Amount.HasValue)
            {
                return Amount.Value.ToString();
            }
            else
            {
                return "NULL";
            } 
        }

        private string GetDateForSQL()
        {
            if (Date.HasValue)
            {
                return Date.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                return "NULL";
            }
        }

        private string GetTypeForSQL()
        {
            if (TypeID.HasValue)
            {
                return TypeID.Value.ToString();
            }
            else
            {
                return "NULL";
            }
        }

        private string GetCategoryForSQL()
        {
            if (Category.HasValue)
            {
                return Category.Value.ToString();
            }
            else
            {
                return "NULL";
            }
        }

        private string GetRecordByForSQL()
        {
            if (RecordBy.Length != 0)
            {
                return RecordBy;
            }
            else
            {
                return "NULL";
            }
        }

        private string GetRecordDateForSQL()
        {
            if (RecordDate.HasValue)
            {
                return RecordDate.Value.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
        }
    }

    public enum TransactionType
    {
        Income = 1,
        Expense
    }
}
