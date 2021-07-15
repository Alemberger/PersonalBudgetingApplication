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
    class Lookup
    {
        private int? _lookupId;
        private string _description;

        public int? LookupID { get => _lookupId; set => _lookupId = value; }
        public string Description { get => _description; set => _description = value; }
        public string RecordBy { get; set; }
        public DateTime? RecordDate { get; set; }

        public EntityState State { get; set; }

        public LookupType Type { get; set; }

        public Lookup() 
        {
            LookupID = -1;
            State = EntityState.Added;
        }

        public Lookup(int id)
        {
            if (id < 1) { throw new ArgumentException("Must provide a valid LookupID"); }

            LookupID = id;

            GetRecord();
        }

        private void GetRecord()
        {
            if (!LookupID.HasValue || LookupID.Value < 1) { throw new ArgumentException("Cannot retrieve a record without a valid LookupID"); }

            var recordQuery = string.Format("SELECT TypeID, Description, RecordBy, RecordDate FROM lookup WHERE LookupID = {0}", GetLookupIDForSQL());
            var recordColumns = new List<DataColumn> { new DataColumn("TypeID", typeof(int)), new DataColumn("Description", typeof(string)), new DataColumn("RecordBy", typeof(string)), new DataColumn("RecordDate", typeof(DateTime)) };

            var result = DataAccess.ExecuteSelectQuery(recordQuery, recordColumns);
        }

        public void SaveChanges()
        {
            if (Description is null || Description.Length == 0) { throw new InvalidOperationException("Must provide a lookup description before saving"); }

            switch (State)
            {
                case EntityState.Added:
                    if (LookupID > 0) { throw new ArgumentException("Cannot insert a record that already has a valid LookupID"); }
                    int id = Insert();
                    if (id < 1) { throw new DataException("Insert returned an invalid LookupID"); }
                    LookupID = id;
                    State = EntityState.Unchanged;
                    break;
                case EntityState.Modified:
                    if (LookupID < 1) { throw new ArgumentException("Cannot update a record without a valid LookupID"); }
                    Update();
                    State = EntityState.Unchanged;
                    break;
                case EntityState.Deleted:
                    if (LookupID < 1) { throw new ArgumentException("Cannot delete a record without a valid LookupID"); }
                    Delete();
                    State = EntityState.Detached;
                    break;
                case EntityState.Detached:
                    throw new InvalidOperationException("Cannot save changes to a record that has already been deleted");
                case EntityState.Unchanged:
                    break;
                default:
                    throw new InvalidOperationException("Invalid State provided"); 
            }
        }

        private int Insert()
        {
            var insertQuery = string.Format("INSERT INTO tblLookup (TypeID, Description, RecordBy, RecordDate) VALUEs ({0}, {1}, {2}, {3})", GetLookupTypeForSQL(), GetDescriptionForSQL(), GetRecordByForSQL(), GetRecordDateForSQL());

            DataAccess.ExecuteNonQuery(insertQuery);

            int? lookupId = null;

            string idSelect = "SELECT LookupID FROM tblLookup ORDER BY LookupID DESC LIMIT 1";
            List<DataColumn> idColumns = new List<DataColumn> { new DataColumn("LookupID", typeof(int)) };

            var result = DataAccess.ExecuteSelectQuery(idSelect, idColumns);

            if (result.Rows.Count != 1) { throw new DataException("Unexpected number of results returned"); }

            lookupId = (int)result.Rows[0].ItemArray[0];

            if (!lookupId.HasValue || lookupId.Value < 1) { throw new DataException("Select returned an invalid LookupID"); }

            return lookupId.Value;
        }

        private void Update()
        {
            var updateQuery = string.Format("UPDATE tblLookup SET TypeID = {0}, Description = {1}, RecordBy = {2}, RecordDate = {3} WHERE LookupID = {4}", GetLookupTypeForSQL(), GetDescriptionForSQL(), GetRecordByForSQL(), GetRecordDateForSQL());
            DataAccess.ExecuteNonQuery(updateQuery);
        }

        private void Delete()
        {
            var deleteQuery = string.Format("DELETE FROM tblLookup WHERE LookupID = {0}", GetLookupIDForSQL());
            DataAccess.ExecuteNonQuery(deleteQuery);
        }

        private string GetLookupIDForSQL()
        {
            if (LookupID.HasValue && LookupID > 1)
            {
                return LookupID.Value.ToString();
            }
            else
            {
                return "NULL";
            }
        }

        private string GetDescriptionForSQL()
        {
            if (Description.Length == 0)
            {
                return "NULL";
            }
            else
            {
                return string.Format("'{0}'", Description);
            }
        }

        private string GetLookupTypeForSQL()
        {
            if (Type is null || Type.LookupTypeID < 1)
            {
                return "NULL";
            }
            else
            {
                return Type.LookupTypeID.ToString();
            }
        }

        private string GetRecordByForSQL()
        {
            if (RecordBy.Length == 0)
            {
                return "NULL";
            }
            else
            {
                return string.Format("'{0}'", RecordBy);
            }
        }

        private string GetRecordDateForSQL()
        {
            if (RecordDate.HasValue)
            {
                return string.Format("'{0}'", RecordDate.Value.ToString("yyyy-MM-dd HH:mm"));
            }
            else
            {
                return "NULL";
            }
        }
    }
}
