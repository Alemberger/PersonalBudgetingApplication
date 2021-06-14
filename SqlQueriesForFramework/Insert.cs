using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    class Insert : IQuery
    {
        static List<QueryClause> _requiredClauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Values, QueryClause.Tables };
        static List<QueryClause> _allowedClauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Values, QueryClause.Tables };

        List<string> QueryFields { get; set; }
        List<string> QueryTables { get; set; }
        List<string> QueryValues { get; set; }

        public QueryType Type => QueryType.Insert;

        public ExecutionType ExecType { get; set; }

        public string QueryString => BuildString();

        public DataSet QueryResults { get; set; }

        public List<QueryClause> Clauses { get; private set; }

        public static bool ValidClause(QueryClause toCheck)
        {
            return _allowedClauses.Contains(toCheck);
        }

        public static bool CheckForRequired(QueryClause[] checkList)
        {
            foreach (var item in _requiredClauses)
            {
                if (!checkList.Contains(item)) { return false; }
            }

            return true;
        }

        public void AddClause(QueryClause clause)
        {
            if (!_allowedClauses.Contains(clause)) { throw new ArgumentException("Clause added is not allowed"); }

            if (Clauses.Contains(clause))
            {
                throw new ArgumentException("Clause is already included");
            }
            else
            {
                Clauses.Add(clause);
            }
        }

        public void AddClause(QueryClause clause, List<object> values)
        {
            AddClause(clause);
            AddValues(clause, values);
        }

        public void AddValues(QueryClause clause, List<object> values)
        {
            if (!Clauses.Contains(clause)) { throw new InvalidOperationException("Clause type is not included in clauses"); }

            ValidateObjectList(clause, values);

            switch (clause)
            {
                case QueryClause.Fields:
                    var fBinder = new List<string>();
                    foreach (object item in values)
                    {
                        fBinder.Add(item.ToString());
                    }
                    QueryFields = fBinder;
                    break;
                case QueryClause.Tables:
                    if (values.Count != 1) { throw new ArgumentException("Must add one and only one table to the query"); }
                    var tBinder = new List<string>();
                    tBinder.Add(values[0].ToString());
                    QueryTables = tBinder;
                    break;
                case QueryClause.Values:
                    if (QueryFields is null || QueryFields.Count < 1) { throw new InvalidOperationException("Need to add the fields clause first"); }
                    if (values.Count % QueryFields.Count != 0) { throw new ArgumentException("Must add one value for each field in the query"); }

                    for (var i = 0; i < values.Count / QueryFields.Count; i++)
                    {
                        var skip = i * QueryFields.Count;
                        var take = QueryFields.Count;

                        var nextSet = new List<string>();
                        foreach (object item in values.GetRange(skip, take))
                        {
                            nextSet.Add(item.ToString());
                        }
                        LoadValueSet(nextSet);
                    }
                    break;
            }
        }

        public void LoadValueSet(List<string> valueSet)
        {
            List<string> binder;
            if (QueryValues is null)
            {
                binder = new List<string>();
            }
            else
            {
                binder = QueryValues;
            }

            binder.AddRange(valueSet);

            QueryValues = binder;
        }

        public string BuildString()
        {
            if (QueryFields is null || QueryFields.Count < 1 || QueryTables is null || QueryTables.Count != 1) { throw new InvalidOperationException("Must include fields and one table for this query"); }

            var queryString = string.Format("INSERT INTO {0} ({1}", QueryTables[0], QueryFields[0]);

            for (int i = 1; i < QueryFields.Count; i++)
            {
                queryString += string.Format(", {0}", QueryFields[i]);
            }

            queryString += string.Format(") VALUES (");

            for (int i = 0; i < QueryValues.Count / QueryFields.Count; i++)
            {
                if (i != 0) { queryString += "), ("; }
                var skip = i * QueryFields.Count;
                var take = QueryFields.Count;
                var nextSet = QueryValues.GetRange(skip, take);
                queryString += nextSet[0];
                for (int j = 1; j < nextSet.Count; j++)
                {
                    queryString += string.Format(", {0}", nextSet[j]);
                }
            }

            queryString += ")";
            return queryString;
        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }

        public void ValidateObjectList(QueryClause target, List<object> objList)
        {
            if (objList is null || objList.Count < 1) { throw new ArgumentException("Must provide a list containing at least one value"); }

            var valid = true;
            var desiredType = "";

            switch (target)
            {
                case QueryClause.Fields:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
                case QueryClause.Tables:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
                case QueryClause.Values:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
                default:
                    throw new ArgumentException("Invalid clause type");
            }

            if (!valid) { throw new ArgumentException(string.Format("{0} clause requires content of type {1}", target.ToString(), desiredType)); }
        }
    }
}
