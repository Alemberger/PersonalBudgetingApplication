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
        public static List<QueryClause> _requiredClauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Tables };

        public static List<QueryClause> _allowedClauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Tables, QueryClause.Joins, QueryClause.Where, QueryClause.WhereJoins, QueryClause.GroupBy, QueryClause.Having, QueryClause.HavingJoins, QueryClause.OrderBy };

        public QueryType Type => QueryType.Select;

        public ExecutionType ExecType { get; set; }

        public string QueryString { get { return BuildString(); } }

        public DataSet QueryResults { get; set; }

        public List<QueryClause> Clauses { get; private set; }

        List<string> QueryFields { get; set; }
        List<string> QueryTables { get; set; }
        List<Join> QueryJoins { get; set; }
        List<ComparisonComponent> QueryWheres { get; set; }
        List<string> QueryWhereJoins { get; set; }
        List<string> QueryGroupBys { get; set; }
        List<ComparisonComponent> QueryHaving { get; set; }
        List<string> QueryHavingJoins { get; set; }
        List<string> QueryOrderBy { get; set; }

        public Select()
        {
            //Sets the required clauses for the SELECT query
            Clauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Tables };
        }

        public Select(QueryClause[] clauses, Dictionary<QueryClause, List<object>> contents)
        {
            if (clauses.Length < contents.Count) { throw new ArgumentException("More clause contents provided than included clauses"); }
            foreach (var clause in clauses)
            {
                if (!ValidClause(clause))
                {
                    throw new ArgumentException("Invalid clause provided");
                }
            }

            if (!CheckForRequired(clauses)) { throw new ArgumentException("Missing required clauses"); }

            for (int i = 0; i < clauses.Length; i++)
            {
                var currentClause = clauses[i];
                if (contents.ContainsKey(currentClause))
                {
                    AddClause(currentClause, contents[currentClause]);
                }
                else
                {
                    AddClause(currentClause);
                }
            }
        }

        public static bool ValidClause(QueryClause check)
        {
            return _allowedClauses.Contains(check);
        }

        public static bool CheckForRequired(QueryClause[] list)
        {
            foreach (var reqType in _requiredClauses)
            {
                if (!list.Contains(reqType))
                {
                    return false;
                }
            }

            return true;
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

        public void ValidateObjectList(QueryClause target, List<object> objList)
        {
            if (objList.Count < 1) { throw new InvalidOperationException("Must provide at least one object"); }

            bool valid = true;
            string desiredType = "";

            switch (target)
            {
                case QueryClause.Fields:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
                case QueryClause.Tables:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
                case QueryClause.Joins:
                    if (objList[0].GetType() != typeof(Join)) { valid = false; desiredType = "Join"; }
                    break;
                case QueryClause.Where:
                    if (objList[0].GetType() != typeof(ComparisonComponent)) { valid = false; desiredType = "Comparison Component"; }
                    break;
                case QueryClause.WhereJoins:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
                case QueryClause.GroupBy:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
                case QueryClause.Having:
                    if (objList[0].GetType() != typeof(ComparisonComponent)) { valid = false; desiredType = "Comparison Component"; }
                    break;
                case QueryClause.HavingJoins:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
                case QueryClause.OrderBy:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
            }

            if (!valid) { throw new ArgumentException(string.Format("Must provide a {0} list for {1}s", desiredType, target.ToString())); }
        }

        public void AddValues(QueryClause clause, List<object> values)
        {
            if (!_allowedClauses.Contains(clause)) { throw new ArgumentException("This clause is not allowed for this query"); }

            if (!Clauses.Contains(clause))
            {
                throw new ArgumentException("Clause is not included in the query");
            }

            ValidateObjectList(clause, values);

            switch (clause)
            {
                case QueryClause.Fields:
                    var fBinding = new List<string>();
                    foreach (object item in values)
                    {
                        fBinding.Add(item.ToString());
                    }
                    QueryFields = fBinding;
                    break;
                case QueryClause.Tables:
                    var tBinding = new List<string>();
                    foreach (object item in values)
                    {
                        tBinding.Add(item.ToString());
                    }
                    QueryTables = tBinding;
                    break;
                case QueryClause.Joins:
                    var jBinding = new List<Join>();
                    foreach (object item in values)
                    {
                        jBinding.Add((Join)item);
                    }
                    QueryJoins = jBinding;
                    break;
                case QueryClause.Where:
                    var wBinding = new List<ComparisonComponent>();
                    foreach (object item in values)
                    {
                        wBinding.Add((ComparisonComponent)item);
                    }
                    QueryWheres = wBinding;
                    break;
                case QueryClause.WhereJoins:
                    var wjBinding = new List<string>();
                    foreach (object item in values)
                    {
                        wjBinding.Add(item.ToString());
                    }
                    QueryWhereJoins = wjBinding;
                    break;
                case QueryClause.GroupBy:
                    var gbBinding = new List<string>();
                    foreach (object item in values)
                    {
                        gbBinding.Add(item.ToString());
                    }
                    QueryGroupBys = gbBinding;
                    break;
                case QueryClause.Having:
                    var hBinding = new List<ComparisonComponent>();
                    foreach (object item in values)
                    {
                        hBinding.Add((ComparisonComponent)item);
                    }
                    QueryHaving = hBinding;
                    break;
                case QueryClause.HavingJoins:
                    var hjBinding = new List<string>();
                    foreach (object item in values)
                    {
                        hjBinding.Add(item.ToString());
                    }
                    QueryHavingJoins = hjBinding;
                    break;
                case QueryClause.OrderBy:
                    var obBinding = new List<string>();
                    foreach (object item in values)
                    {
                        obBinding.Add(item.ToString());
                    }
                    QueryOrderBy = obBinding;
                    break;
            }
        }

        public void AddClause(QueryClause clause, List<object> values)
        {
            AddClause(clause);

            AddValues(clause, values);
        }

        public string BuildString()
        {
            var queryString = string.Format("SELECT {0} FROM {1}", WriteSelectClause(), WriteFromClause());

            if (Clauses.Contains(QueryClause.Where))
            {
                queryString += string.Format(" WHERE {0}", WriteWhereClause());
            }

            if (Clauses.Contains(QueryClause.GroupBy))
            {
                queryString += string.Format(" GROUP BY {0}", WriteGroupByClause());
            }

            if (Clauses.Contains(QueryClause.Having))
            {
                queryString += string.Format(" HAVING {0}", WriteHavingClause());
            }

            if (Clauses.Contains(QueryClause.OrderBy))
            {
                queryString += string.Format(" ORDER BY {0}", WriteOrderByClause());
            }

            return queryString;
        }

        private string WriteSelectClause()
        {
            if (QueryFields.Count < 1) { throw new InvalidOperationException("Must provide at least one field for the query"); }

            var queryPart = QueryFields[0];

            for (int i = 1; i < QueryFields.Count; i++)
            {
                queryPart += string.Format(", {0}", QueryFields[i]);
            }

            return queryPart;
        }

        private string WriteFromClause()
        {
            if (QueryTables.Count < 1) { throw new InvalidOperationException("Must provide at least one table for the query"); }

            if (QueryTables.Count > 1 && (QueryJoins is null || QueryJoins.Count < 1)) { throw new InvalidOperationException("Must provide joins when selecting from multiple tables"); }

            if (QueryTables.Count > 1 && (QueryJoins.Count != QueryTables.Count - 1)) { throw new ArgumentException("Must provide one join for each additional table in the query"); }

            var queryPart = QueryTables[0];

            for (int i = 1; i < QueryTables.Count; i++)
            {
                var table = QueryTables[i];
                var join = QueryJoins[i - 1];

                queryPart += string.Format("{0} JOIN {1} ON {2}", join.Type.ToString(), table, join.OnClauses[0].ToString());

                for (int j = 1; j < join.OnClauses.Count; j++)
                {
                    queryPart += string.Format(" {0} {1}", join.ClauseJoiners[j - 1], join.OnClauses[j]);
                }
            }

            return queryPart;
        }

        private string WriteWhereClause()
        {
            if (QueryWheres is null || QueryWheres.Count < 1) { throw new InvalidOperationException("Must provide a set of Where clauses to write this query part"); }

            if (QueryWheres.Count > 1 && (QueryWhereJoins is null || QueryWhereJoins.Count < 1)) { throw new InvalidOperationException("Must provide where joins if using more than one where clause"); }

            if (QueryWheres.Count > 1 && (QueryWhereJoins.Count != QueryWheres.Count - 1)) { throw new ArgumentException("Must provide one where join for every additional where clause"); }

            var queryPart = QueryWheres[0].ToString();

            for (int i = 1; i < QueryWheres.Count; i++)
            {
                queryPart += string.Format(" {0} {1}", QueryWhereJoins[i - 1], QueryWheres[i]);
            }

            return queryPart;
        }

        private string WriteGroupByClause()
        {
            if (QueryGroupBys is null || QueryGroupBys.Count < 1) { throw new InvalidOperationException("Must provide a set of group by clauses"); }

            var queryPart = QueryGroupBys[0];

            for (int i = 1; i < QueryGroupBys.Count; i++)
            {
                queryPart += string.Format(", {0}", QueryGroupBys[i]);
            }

            return queryPart;
        }

        private string WriteHavingClause()
        {
            if (QueryHaving is null || QueryHaving.Count < 1) { throw new InvalidOperationException("Must proivde having clauses"); }

            if (QueryHaving.Count > 1 && (QueryHavingJoins is null || QueryHavingJoins.Count < 1)) { throw new InvalidOperationException("Must provide HavingJoins if using multiple having clauses"); }

            if (QueryHaving.Count > 1 && (QueryHavingJoins.Count != QueryHaving.Count - 1)) { throw new ArgumentException("Must provide one HavingJoin for every additional having clause"); }

            var queryPart = QueryHaving[0].ToString();

            for (int i = 1; i < QueryHaving.Count; i++)
            {
                queryPart += string.Format(" {0} {1}", QueryHavingJoins[i - 1] + QueryHaving[i].ToString());
            }

            return queryPart;
        }

        private string WriteOrderByClause()
        {
            if (QueryOrderBy is null || QueryOrderBy.Count < 1) { throw new InvalidOperationException("Must provide order by clauses"); }

            var queryPart = QueryOrderBy[0];

            for (int i = 1; i < QueryOrderBy.Count; i++)
            {
                queryPart += string.Format(", {0}", QueryOrderBy[i]);
            }

            return queryPart;
        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}
