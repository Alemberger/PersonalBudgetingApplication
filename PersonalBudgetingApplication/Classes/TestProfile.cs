using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class TestProfile : AppObject
    {
        private int _id;
        private string _objectType;

        public override int ID
        {
            get { return GetID(); }
            set { SetID(value); }
        }

        public override string ObjectType
        {
            get { return GetObjType(); }
        }

        private int GetID()
        {
            return _id;
        }

        private void SetID(int newId)
        {
            _id = newId;
        }

        private string GetObjType()
        {
            return _objectType;
        }
    }
}
