using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    public interface IQuery
    {
        ExecutionType ExecType { get; set; }

        string QueryString { get; }

        DataSet QueryResults { get; set; }

        List<QueryClause> Clauses { get; }

        Dictionary<QueryClause, List<string>> ClauseValues { get; }

        void AddClause(QueryClause clause);

        void AddValues(QueryClause clause, List<string> values);

        void AddClause(QueryClause clause, List<string> values);

        string BuildString();

        bool Execute();
    }

    public enum QueryClause
    {
        Fields = 1,
        Values,
        Tables,
        Joins,
        Where,
        GroupBy,
        Having,
        OrderBy
    }

    public enum ExecutionType
    {
        NonQuery = 1,
        SingleQuery,
        MultiQuery
    }

    public enum QueryType
    {
        Select = 1,
        Insert,
        Update,
        Delete
    }
}
