using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Core_Objects
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Name { get; set; }
        public double? Amount { get; set; }
        public int? Type { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string RecordBy { get; set; }
        public DateTime? RecordDate { get; set; }

        public Profile Profile { get; set; }

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
        }

        private void GetAccount()
        {
            //Confirm a valid AccountID is set
            //Connect to the DB
            //Retrieve the record for the account with the AccountID of the object
            //Parse the record values into the object
            //Cleanup
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
                    Input();
                    break;
                case EntityState.Modified:
                    Update();
                    break;
                case EntityState.Deleted:
                    Delete();
                    break;
                default:
                    break;
            }
            //Cleanup
            State = EntityState.Unchanged;
        }

        private void Input()
        {
            var access = new Classes.DataAccess();


        }

        private void Update()
        {
            throw new NotImplementedException();
        }

        private void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
