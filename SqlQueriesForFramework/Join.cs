using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    public class Join
    {
        public JoinType Type { get; set; } = JoinType.Unset;

        public List<ComparisonComponent> OnClauses { get; set; }

        public List<string> ClauseJoiners { get; set; }

        public Join()
        {
        }

        public Join(JoinType type)
        {
            Type = type;
        }

        public void AddOnClause(string value1, string value2, string op)
        {
            var component = new ComparisonComponent(value1, value2, op);
            OnClauses.Add(component);
        }
    }
}
