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
    public class Account
    {
        public int AccountID { get; set; }
        public string Name
        {
            get { return Name; }
            set { Name = value; State = EntityState.Modified; }
        }
        public double? Amount { get { return Amount; } set { Amount = value; State = EntityState.Modified; } }
        public int? Type { get { return Type; } set { Type = value; State = EntityState.Modified; } }
        public DateTime? LastUpdateDate 
        { 
            get 
            { 
                return LastUpdateDate; 
            }
            set
            {
                LastUpdateDate = value;
                State = EntityState.Modified;
            } 
        }
        public string RecordBy { get; set; }
        public DateTime? RecordDate { get; set; }

        public Profile Profile { get; set; }
        public List<Transaction> Transactions { get; set; }

        public EntityState State { get; set; }

        public Account()
        {
            AccountID = -1;
        }

        public Account(int id)
        {
            if (id < 1) { throw new ArgumentException("Must provide a valid UID for the account"); }

            AccountID = id;

            GetAccount();

            State = EntityState.Unchanged;
        }

        private string GetAccountID()
        {
            if (AccountID < 1) { return ""; }
            else { return AccountID.ToString(); }
        }

        private string GetAccountName()
        {
            return Name;
        }

        private string GetCurrentBalance()
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

        private string GetAccountType()
        {
            if (Type.HasValue)
            {
                return Type.Value.ToString();
            }
            else
            {
                return "NULL";
            }
        }

        private string GetLastUpdateDate()
        {
            if (LastUpdateDate.HasValue)
            {
                return LastUpdateDate.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                return "NULL";
            }
        }

        private string GetProfileID()
        {
            if (!(Profile is null))
            {
                if (Profile.ProfileID > 0)
                {
                    return Profile.ProfileID.ToString();
                }
            }

            return "NULL";
        }

        private string GetRecordBy()
        {
            return RecordBy;
        }

        private string GetRecordDate()
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

        private void GetAccount()
        {
            //Confirm a valid AccountID is set
            if (AccountID < 1) { throw new InvalidOperationException("Cannot retrieve a record without a valid AccountID"); }

            try
            {
                var selectQuery = string.Format("SELECT Acc_name, Acc_amount, Acc_LastUpdateDate, Acc_type, RecordBy, RecordDate FROM tblAccount WHERE AccountID = {0}", GetAccountID());
                var selectColumns = new List<DataColumn> { new DataColumn("AccountName", typeof(string)), new DataColumn("Amount", typeof(double)), new DataColumn("LastUpdateDate", typeof(DateTime)), new DataColumn("Type", typeof(int)), new DataColumn("RecordBy", typeof(string)), new DataColumn("RecordDate", typeof(DateTime)) };

                var result = DataAccess.ExecuteSelectQuery(selectQuery, selectColumns);

                if (result.Rows.Count != 1) { throw new DataException("Unexpected number of rows returned by the query:" + result.Rows.Count.ToString()); }

                Name = result.Rows[0].ItemArray[0].ToString();
                Amount = (double?)result.Rows[0].ItemArray[1];
                LastUpdateDate = (DateTime?)result.Rows[0].ItemArray[2];
                Type = (int?)result.Rows[0].ItemArray[3];
                RecordBy = result.Rows[0].ItemArray[4].ToString();
                RecordDate = (DateTime?)result.Rows[0].ItemArray[5];
            }
            catch (Exception ex) { throw ex; }

            var transactionQuery = string.Format("SELECT TransactionID FROM tblTransaction WHERE AccountID = {0}", GetAccountID());
            var transactionColumns = new List<DataColumn> { new DataColumn("TransactionID", typeof(int)) };

            var transactions = DataAccess.ExecuteSelectQuery(transactionQuery, transactionColumns);

            var linkedTransactions = new List<Transaction>();

            foreach (DataRow row in transactions.Rows)
            {
                var newTransaction = new Transaction((int)row[0]);
                linkedTransactions.Add(newTransaction);
            }

            Transactions = linkedTransactions;
        }

        public void Save()
        {
            //Confirm account contains valid required values
            var valid = true;
            if (Name is null || Name == "") { valid = false; }
            if (Amount is null) { valid = false; }
            if (Type is null || Type < 1) { valid = false; }
            if (LastUpdateDate is null || LastUpdateDate.Value < new DateTime(2020, 1, 1)) { valid = false; }
            if (!valid) { throw new InvalidOperationException("Missing required parameters"); }

            //Identify and execute what kind of save will be done
            switch (State)
            {
                case EntityState.Added:
                    if (AccountID > 0) { throw new InvalidOperationException("Cannot insert a record that already has a valid AccountID"); }
                    var id = Input();
                    AccountID = id;
                    break;
                case EntityState.Modified:
                    if (AccountID < 1) { throw new InvalidOperationException("Cannot update a record that does not have a valid AccountID"); }
                    Update();
                    break;
                case EntityState.Deleted:
                    if (AccountID < 1) { throw new InvalidOperationException("Cannot delete a record that does not have a valid AccountID"); }
                    Delete();
                    break;
                default:
                    break;
            }
            //Cleanup
            State = EntityState.Unchanged;
        }

        /// <summary>
        /// Submits a new record to the database and returns the newly created AccountID
        /// </summary>
        /// <returns></returns>
        private int Input()
        {
            //Write the insert query string
            var query =
                string.Format("INSERT INTO tblAccounts (Acc_Name, Acc_Amount, Acc_Type, Acc_LastUpdateDate, ProfileID, RecordBy, RecordDate) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                GetAccountName(), GetCurrentBalance(), GetAccountType(), GetLastUpdateDate(), GetProfileID(), GetRecordBy(), GetRecordDate());

            //Execute the insert query
            try
            {
                DataAccess.ExecuteNonQuery(query);
            }
            catch (Exception ex) { var Type = ex.GetType(); throw ex; }

            //Get the most recently added AccountID 
            try
            {
                var selectString = "SELECT AccountID FROM tblAccounts ORDER BY AccountID DESC LIMIT 1";
                var selectColumns = new List<DataColumn> { new DataColumn("AccountID", typeof(int)) };

                DataTable result = DataAccess.ExecuteSelectQuery(selectString, selectColumns);

                if (result.Rows.Count != 1) { throw new DataException("Incorrect number of rows returned"); }

                return Convert.ToInt32(result.Rows[0].ItemArray[0]);
            }
            catch (Exception ex) { var Type = ex.GetType(); throw ex; }
        }

        private void Update()
        {
            var query =
                string.Format("UPDATE tblAccounts SET Acc_Name = {0}, Acc_Amount = {1}, Acc_Type = {2}, Acc_LastUpdateDate = {3}, ProfileID = {4}, RecordBy = {5}, RecordDate = {6} WHERE AccountID = {7}",
                GetAccountName(), GetCurrentBalance(), GetAccountType(), GetLastUpdateDate(), GetProfileID(), GetRecordBy(), GetRecordDate(), GetAccountID());

            try
            {
                DataAccess.ExecuteNonQuery(query);
            }
            catch (Exception ex) { var Type = ex.GetType(); }
        }

        private void Delete()
        {
            var query =
                string.Format("DELETE FROM tblAccounts WHERE AccountID = {0}", GetAccountID());

            try
            {
                DataAccess.ExecuteNonQuery(query);
            }
            catch (Exception ex) { var Type = ex.GetType(); }
        }
    }
}
