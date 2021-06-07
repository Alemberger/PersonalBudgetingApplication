using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public abstract class AppObject
    {
        public abstract string ObjectType { get; }

        public abstract int ID { get; set; }
    }
}
