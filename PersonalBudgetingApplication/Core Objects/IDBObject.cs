using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Core_Objects
{
    interface IDBObject
    {
        int DBID { get; set; }

        ObjectState State { get; set; }

        DateTime? RecordDate { get; set; }

        string RecordBy { get; set; }

        void CreateObject();

        void GetObject(int id);

        bool SaveChanges();
    }

    public enum ObjectState
    {
        Unchanged = 0,
        Added,
        Modified,
        Deleted,
        Detached
    }
}
