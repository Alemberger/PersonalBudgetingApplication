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
    public class Profile
    {
        private int _profileId;
        private string _name;
        public int ProfileID 
        {
            get => _profileId;
            set => _profileId = value;
        }
        public string Name 
        {
            get => _name;
            set
            {
                _name = value;
                if (State != EntityState.Added)
                {
                    State = EntityState.Modified;
                }
            }
        }
        public string RecordBy { get; set; }
        public DateTime? RecordDate { get; set; }

        public EntityState State { get; set; }

        public List<Account> Accounts { get; set; }

        public Profile()
        {
            ProfileID = -1;
            State = EntityState.Added;
        }

        public Profile(int profileId)
        {
            ProfileID = profileId;

            GetRecord();
            State = EntityState.Unchanged;
        }

        private string GetProfileID()
        {
            return ProfileID.ToString();
        }

        private string GetProfileName()
        {
            if (Name is null) { return ""; }
            else { return Name; }
        }

        private string GetRecordBy()
        {
            if (RecordBy is null) { return ""; }
            else { return RecordBy; }
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

        private void GetRecord()
        {
            if (ProfileID < 1) { throw new ArgumentException("Invalid ProfileID provided"); }

            var selectString = "SELECT ProfileName, RecordBy, RecordDate FROM tblProfiles WHERE ProfileID = " + GetProfileID();
            var selectColumns = new List<DataColumn> { new DataColumn("ProfileName", typeof(string)), new DataColumn("RecordBy", typeof(string)), new DataColumn("RecordDate", typeof(DateTime)) };

            try
            {
                var results = DataAccess.ExecuteSelectQuery(selectString, selectColumns);

                if (results.Rows.Count != 1) { throw new DataException("Query returned incorrect number of records"); }

                Name = results.Rows[0].ItemArray[0].ToString();
                RecordBy = results.Rows[0].ItemArray[1].ToString();
                RecordDate = DateTime.Parse(results.Rows[0].ItemArray[2].ToString());
            }
            catch (Exception ex) { throw ex; }

            var accountSelect = "SELECT AccountID FROM tblAccount WHERE ProfileID = " + GetProfileID();
            var accountColumns = new List<DataColumn> { new DataColumn("AccountID", typeof(int)) };

            var accounts = DataAccess.ExecuteSelectQuery(accountSelect, accountColumns);

            var linkedAccounts = new List<Account>();
            if (accounts.Rows.Count > 0)
            {
                foreach (DataRow row in accounts.Rows)
                {
                    var newAccount = new Account((int)row[0]);
                    linkedAccounts.Add(newAccount);
                }
            }
            Accounts = linkedAccounts;
        }

        public void SaveChanges()
        {
            //Confirm all required values are provided
            if (Name is null || Name.Length == 0) { throw new InvalidOperationException("Must proivde a name"); }
            //select the operation to take based on the object state
            switch (State)
            {
                case EntityState.Added:
                    if (ProfileID > 0) { throw new ArgumentException("Cannot provide a positive ProfileID for records to be added"); }
                    var id = Insert();
                    ProfileID = id;
                    break;
                case EntityState.Modified:
                    if (ProfileID < 1) { throw new ArgumentException("Cannot update a record without a valid ProfileID"); }
                    Update();
                    break;
                case EntityState.Deleted:
                    if (ProfileID < 1) { throw new ArgumentException("Cannot delete a record without a valid ProfileID"); }
                    Delete();
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    throw new InvalidOperationException("Unhandled Entity state provided");
            }
            //cleanup
            State = EntityState.Unchanged;
        }

        private int Insert()
        {
            var insertQuery = string.Format("INSERT INTO tblProfile (ProfileName, RecordBy, RecordDate) VALUES ('{0}', '{1}', '{2}')",
                GetProfileName(), GetRecordBy(), GetRecordDate());

            try
            {
                DataAccess.ExecuteNonQuery(insertQuery);
            }
            catch (Exception ex) { throw ex; }

            try
            {
                var selectQuery = "SELECT ProfileID FROM tblProfile ORDER BY ProfileID DESC LIMIT 1";
                var selectColumns = new List<DataColumn> { new DataColumn("ProfileID", typeof(int)) };

                var result = DataAccess.ExecuteSelectQuery(selectQuery, selectColumns);

                if (result.Rows.Count != 1) { throw new DataException("Unexpected number of records returned"); }

                return Convert.ToInt32(result.Rows[0].ItemArray[0]);
            }
            catch (Exception ex) { throw ex; }
        }

        private void Update()
        {
            var updateQuery = string.Format("UPDATE tblProfile SET ProfileName = '{0}', RecordBy = '{1}', RecordDate = '{2}' WHERE ProfileID = {3}",
                GetProfileName(), GetRecordBy(), GetRecordDate(), GetProfileID());
            try
            {
                DataAccess.ExecuteNonQuery(updateQuery);
            }
            catch (Exception ex) { throw ex; }
        }

        private void Delete()
        {
            var deleteQuery = string.Format("DELETE FROM tblProfile WHERE ProfileID = {0}", GetProfileID());

            try
            {
                DataAccess.ExecuteNonQuery(deleteQuery);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
