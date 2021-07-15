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
    class LookupType
    {
        private int _lookupTypeId;
        private string _lookupTypeName;
        private string _recordBy;
        private DateTime? _recordDate;

        public int LookupTypeID { get => _lookupTypeId; set => _lookupTypeId = value; }
        public string LookupTypeName { get => _lookupTypeName; set => _lookupTypeName = value; }
        public string RecordBy { get => _recordBy; set => _recordBy = value; }
        public DateTime? RecordDate { get => _recordDate; set => _recordDate = value; }
        
        public EntityState State { get; set; }

        public List<Lookup> Lookups { get; private set; }

        public LookupType()
        {
            LookupTypeID = -1;
        }

        public LookupType(int id)
        {
            if (id < 1) { throw new ArgumentException("Must provide a valid LookupTypeID"); }

            LookupTypeID = id;

            GetRecord();
        }

        public void GetRecord()
        {
            if (LookupTypeID < 1) { throw new ArgumentException("Cannot retrieve a record without a valid TypeID"); }

            var recordQuery = string.Format("SELECT TypeName, RecordBy, RecordDate FROM tblLookupType WHERE TypeID = {0}", GetLookupTypeIDForSQL());
            var recordColumns = new List<DataColumn> { new DataColumn("TypeName", typeof(string)), new DataColumn("RecordBy", typeof(string)), new DataColumn("RecordDate", typeof(DateTime)) };

            var result = DataAccess.ExecuteSelectQuery(recordQuery, recordColumns);

            if (result.Rows.Count != 1) { throw new DataException("Unexpected number of record returned"); }

            LookupTypeName = result.Rows[0].ItemArray[0].ToString();
            RecordBy = result.Rows[0].ItemArray[1].ToString();
            RecordDate = (DateTime)result.Rows[0].ItemArray[2];
        }

        public List<Lookup> GetLookups()
        {
            if (LookupTypeID < 1) { throw new ArgumentException("Cannot retrieve lookup records without a valid TypeID"); }

            var lookups = new List<Lookup>();

            var lookupQuery = string.Format("SELECT LookupID FROM lookups WHERE TypeID = {0}", GetLookupTypeIDForSQL());
            var lookupColumns = new List<DataColumn> { new DataColumn("LookupID", typeof(int)) };

            var results = DataAccess.ExecuteSelectQuery(lookupQuery, lookupColumns);

            foreach (DataRow row in results.Rows)
            {
                int lookupId = (int)row[0];

                if (lookupId < 1) { throw new DataException("Invalid LookupID returned by query"); }

                Lookup record = new Lookup(lookupId);

                lookups.Add(record);
            }

            return lookups;
        }

        public void SaveChanges()
        {
            if (LookupTypeName is null || LookupTypeName.Length == 0) { throw new InvalidOperationException("Must provide a Type Name"); }

            switch (State)
            {
                case EntityState.Added:
                    if (LookupTypeID > 0) { throw new ArgumentException("Cannot insert a record with a valid LookupTypeID"); }
                    int id = Insert();
                    LookupTypeID = id;
                    break;
                case EntityState.Modified:
                    if (LookupTypeID < 1) { throw new ArgumentException("Cannot update a record without a valid LookupTypeID"); }
                    Update();
                    break;
                case EntityState.Deleted:
                    if (LookupTypeID < 1) { throw new ArgumentException("Cannot delete a reocrd without a valid LookupTypeID"); }
                    Delete();
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Detached:
                    throw new InvalidOperationException("This record has already been deleted from the database");
            }

            State = EntityState.Unchanged;
        }

        private int Insert()
        {
            var query = string.Format("INSERT INTO tblLookupType (TypeName, RecordBy, RecordDate) VALUES ({0}'], {1}, {2})", GetLookupTypeNameForSQL(), GetRecordByForSQL(), GetRecordDateForSQL());

            DataAccess.ExecuteNonQuery(query);

            var idSelect = "SELECT TypeID FROM tblLookupType ORDER BY TypeID DESC LIMIT 1";
            var idColumns = new List<DataColumn> { new DataColumn("TypeID", typeof(int)) };

            var result = DataAccess.ExecuteSelectQuery(idSelect, idColumns);

            if (result.Rows.Count != 1) { throw new DataException("Unexpected number of records returned"); }

            int id = -1;

            foreach (DataRow row in result.Rows)
            {
                id = (int)row[0];
            }

            return id;
        }

        private void Update()
        {
            var updateQuery = string.Format("UPDATE tblLookupType SET TypeName = {0}, RecordBy = {1}, RecordDate = {2} WHERE TypeID = {3}", GetLookupTypeNameForSQL(), GetRecordByForSQL(), GetRecordDateForSQL(), GetLookupTypeIDForSQL());

            DataAccess.ExecuteNonQuery(updateQuery);
        }

        private void Delete()
        {
            var deleteQuery = string.Format("DELETE FROM tblLookupType WHERE TypeID = {0}", GetLookupTypeIDForSQL());

            DataAccess.ExecuteNonQuery(deleteQuery);
        }

        private string GetLookupTypeIDForSQL()
        {
            return LookupTypeID.ToString(); 
        }

        private string GetLookupTypeNameForSQL()
        {
            if (LookupTypeName is null || LookupTypeName.Length == 0)
            {
                return "NULL";
            }
            else
            {
                return string.Format("'{0}'", LookupTypeName);
            }
        }

        private string GetRecordByForSQL()
        {
            if (RecordBy is null || RecordBy.Length == 0)
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
