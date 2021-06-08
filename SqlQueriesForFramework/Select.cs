using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    public class Select : IQuery
    {
        public List<QueryClause> _requiredClauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Tables };

        public List<QueryClause> _allowedClauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Tables, QueryClause.Joins, QueryClause.Where, QueryClause.GroupBy, QueryClause.Having, QueryClause.OrderBy };

        public ExecutionType ExecType { get; set; }

        public string QueryString { get { return BuildString(); } }

        public DataSet QueryResults { get; set; }

        public List<QueryClause> Clauses { get; }

        public Dictionary<QueryClause, List<string>> ClauseValues { get; private set; }

        public Select()
        {
            //Sets the required clauses for the SELECT query
            Clauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Tables };
        }

        public void AddClause(QueryClause clause)
        {
            if (!_allowedClauses.Contains(clause)) { throw new ArgumentException("This clause is not allowed for this query"); }

            if (!Clauses.Contains(clause))
            {
                Clauses.Add(clause);
            }
            else
            {
                throw new ArgumentException("Clause is already included in the query");
            }
        }

        public void AddValues(QueryClause clause, List<string> values)
        {
            if (!_allowedClauses.Contains(clause)) { throw new ArgumentException("This clause is not allowed for this query"); }

            if (!Clauses.Contains(clause))
            {
                throw new ArgumentException("Clause is not included in the query");
            }
            else
            {
                ClauseValues.Add(clause, values);
            }
        }

        public void AddClause(QueryClause clause, List<string> values)
        {
            AddClause(clause);

            AddValues(clause, values);
        }

        public string BuildString()
        {
            throw new NotImplementedException();
        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}
