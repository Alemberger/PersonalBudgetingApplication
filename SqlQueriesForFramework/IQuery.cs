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
        QueryType Type { get; }

        ExecutionType ExecType { get; set; }

        string QueryString { get; }

        DataSet QueryResults { get; set; }

        List<QueryClause> Clauses { get; }

        void AddClause(QueryClause clause);

        void AddValues(QueryClause clause, List<object> values);

        void AddClause(QueryClause clause, List<object> values);

        void ValidateObjectList(QueryClause target, List<object> objList);

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
        WhereJoins,
        GroupBy,
        Having,
        HavingJoins,
        OrderBy
    }

    public enum JoinType
    {
        Unset,
        Left,
        Right,
        Inner,
        Outer,
        Full
    }

    public enum ExecutionType
    {
        NonQuery = 1,
        SingleQuery,
        MultiQuery
    }

    public enum QueryType
    {
        Unset = 0,
        Select,
        Insert,
        Update,
        Delete
    }

    public class ComparisonComponent
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Operation { get; set; }

        public ComparisonComponent(string first, string second, string operation)
        {
            Value1 = first;
            Value2 = second;
            Operation = operation;
        }

        public override string ToString()
        {
            return Value1 + " " + Operation + " " + Value2;
        }
    }
}
