using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    public class GroupByBuilder : IClauseBuilder
    {
        public QueryClause ClauseType { get; }

        public string Clause { get { return BuildClause(); } }

        public object[] ClauseParameters { get; set; }

        public GroupByBuilder()
        {
            ClauseType = QueryClause.GroupBy;
        }

        public string BuildClause()
        {
            if (ClauseParameters is null || ClauseParameters.Length < 1)
            {
                throw new InvalidOperationException("Must include clause parameters");
            }

            string output = ClauseParameters[0].ToString();

            for (int i = 1; i < ClauseParameters.Length; i++)
            {
                output += string.Format(", {0}", ClauseParameters[i]);
            }

            return output;
        }
    }
}
