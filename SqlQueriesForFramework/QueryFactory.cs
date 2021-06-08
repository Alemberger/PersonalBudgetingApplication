using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    public class QueryFactory
    {
        public IQuery BuildQuery(QueryType type)
        {
            switch (type)
            {
                case QueryType.Select:
                    return new Select();
                case QueryType.Insert:
                    return new Insert();
                case QueryType.Update:
                    return new Update();
                case QueryType.Delete:
                    return new Delete();
            }

            throw new ArgumentException();
        }
    }
}
