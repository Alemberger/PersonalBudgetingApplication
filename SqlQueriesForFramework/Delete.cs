using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    class Delete : IQuery
    {
        public static List<QueryClause> _requiredClauses = new List<QueryClause> { QueryClause.Tables };
        public static List<QueryClause> _allowedClauses = new List<QueryClause> { QueryClause.Tables, QueryClause.Where, QueryClause.WhereJoins };

        public ExecutionType ExecType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string QueryString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataSet QueryResults { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        List<string> QueryTables { get; set; }
        List<ComparisonComponent> QueryWheres { get; set; }
        List<string> QueryWhereJoins { get; set; }

        public QueryType Type => QueryType.Delete;

        public List<QueryClause> Clauses => throw new NotImplementedException();

        public Dictionary<QueryClause, List<object>> ClauseValues => throw new NotImplementedException();

        public Delete()
        {
        }

        public Delete(string table)
        {
            var tableList = new List<object> { table };
            AddClause(QueryClause.Tables, tableList);
        }

        public Delete(string table, ComparisonComponent where)
        {
            var tableList = new List<object> { table };
            var whereList = new List<object> { where };
            AddClause(QueryClause.Tables, tableList);
            AddClause(QueryClause.Where, whereList);
        }

        public Delete(string table, ComparisonComponent[] wheres, string[] joins)
        {
            if (wheres.Length != joins.Length + 1) { throw new ArgumentException("Must provide one less join than where clauses"); }

            var tableList = new List<object> { table };
            var whereList = new List<object>();
            var joinList = new List<object>();

            foreach (var component in wheres)
            {
                whereList.Add(component);
            }

            foreach (var join in joins)
            {
                joinList.Add(join);
            }

            AddClause(QueryClause.Tables, tableList);
            AddClause(QueryClause.Where, whereList);
            AddClause(QueryClause.WhereJoins, joinList);
        }

        public static bool ValidClause(QueryClause toCheck)
        {
            return _allowedClauses.Contains(toCheck);
        }

        public void AddClause(QueryClause clause)
        {
            //Check the allowed clause types
            if (!_allowedClauses.Contains(clause)) { throw new ArgumentException("Not an allowed clause"); }

            //Check if it is already loaded
            if (Clauses.Contains(clause)) { throw new ArgumentException("Clause has already been added to the query"); }

            //Load the clause
            Clauses.Add(clause);
        }

        public void AddClause(QueryClause clause, List<object> values)
        {
            AddClause(clause);
            AddValues(clause, values);
        }

        public void AddValues(QueryClause clause, List<object> values)
        {
            if (!Clauses.Contains(clause)) { throw new InvalidOperationException("Clause must be included in the query"); }

            ValidateObjectList(clause, values);

            switch (clause)
            {
                case QueryClause.Tables:
                    var tBinder = new List<string>();
                    foreach (var item in values)
                    {
                        tBinder.Add(item.ToString());
                    }
                    QueryTables = tBinder;
                    break;
                case QueryClause.Where:
                    var wBinder = new List<ComparisonComponent>();
                    foreach (var item in values)
                    {
                        wBinder.Add((ComparisonComponent)item);
                    }
                    QueryWheres = wBinder;
                    break;
                case QueryClause.WhereJoins:
                    var jBinder = new List<string>();
                    foreach (var item in values)
                    {
                        jBinder.Add(item.ToString());
                    }
                    QueryWhereJoins = jBinder;
                    break;
            }
        }

        public string BuildString()
        {
            foreach (var clause in _requiredClauses)
            {
                if (!Clauses.Contains(clause)) { throw new InvalidOperationException("Must include required clauses"); }
            }

            var queryString = WriteDeleteClause();

            if (!(QueryWheres is null) && QueryWheres.Count > 1)
            {
                queryString += string.Format(" {0}", WriteWhereClause()); 
            }

            return queryString;
        }

        private string WriteDeleteClause()
        {
            //Confirm that everything is provided that is required
            if (QueryTables is null || QueryTables.Count != 1) { throw new InvalidOperationException("Must provide a table to delete from"); }

            return string.Format("DELETE {0}", QueryTables[0]);
        }

        private string WriteWhereClause()
        {
            if (QueryWheres is null || QueryWheres.Count < 1) { throw new InvalidOperationException("Cannot add Where clause without any components"); }

            if (QueryWheres.Count > 1 && (QueryWhereJoins is null || QueryWhereJoins.Count < 1)) { throw new InvalidOperationException("Need to include joins when using multiple where clauses"); }

            if (QueryWheres.Count > 1 && (QueryWheres.Count != QueryWhereJoins.Count + 1)) { throw new ArgumentException("Need to include one join for every where clause beyond the first"); }

            var whereString = string.Format("WHERE {0}", QueryWheres[0].ToString());

            for (int i = 1; i < QueryWheres.Count; i++)
            {
                whereString += string.Format(" {0} {1}", QueryWhereJoins[i - 1], QueryWheres[i]);
            }

            return whereString;
        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }

        public static bool CheckForRequired(QueryClause[] list)
        {
            foreach (var clause in _requiredClauses)
            {
                if (!list.Contains(clause))
                {
                    return false;
                }
            }

            return true;
        }

        public void ValidateObjectList(QueryClause target, List<object> objList)
        {
            if (objList.Count < 1) { throw new InvalidOperationException("Must provide at least a single item"); }

            var valid = true;
            var desiredType = "";

            switch (target)
            {
                case QueryClause.Tables:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
                case QueryClause.Where:
                    if (objList[0].GetType() != typeof(ComparisonComponent)) { valid = false; desiredType = "Comparison Component"; }
                    break;
                case QueryClause.WhereJoins:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
            }

            if (!valid) { throw new ArgumentException(string.Format("Must provide an object list containing {0} for {1} clause", desiredType, target.ToString())); }
        }
    }
}
