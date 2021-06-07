using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class AppObjectFactory
    {
        public AppObjectFactory() { }

        public AppObject NewObject(string objectType)
        {
            if (objectType == "") { throw new Exception("Invalid objectType"); }

            switch (objectType)
            {
                case "Profile":
                    return new TestProfile();
                default:
                    throw new Exception("Unknown objectType");
            }
        }
    }
}
