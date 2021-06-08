using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    public interface IClauseBuilder
    {
        QueryClause ClauseType { get; }

        string Clause { get; }

        object[] ClauseParameters { get; set; }

        string BuildClause();
    }
}
