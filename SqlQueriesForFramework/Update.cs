using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    class Update : IQuery
    {
        static List<QueryClause> _requiredClauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Tables, QueryClause.Values };
        static List<QueryClause> _allowedClauses = new List<QueryClause> { QueryClause.Fields, QueryClause.Tables, QueryClause.Values, QueryClause.Where, QueryClause.WhereJoins };

        public QueryType Type => QueryType.Update;

        public ExecutionType ExecType { get; set; }

        public string QueryString => BuildString();

        public DataSet QueryResults { get; set; }

        public List<QueryClause> Clauses { get; private set; }

        List<string> QueryFields { get; set; }
        List<string> QueryTables { get; set; }
        List<string> QueryValues { get; set; }
        List<ComparisonComponent> QueryWhere { get; set; }
        List<string> QueryWhereJoins { get; set; }

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
            if (!_allowedClauses.Contains(clause)) { throw new ArgumentException("Clause type not allowed"); }

            if (Clauses.Contains(clause)) { throw new ArgumentException("Clause type already added"); }

            Clauses.Add(clause);
        }

        public void AddClause(QueryClause clause, List<object> values)
        {
            AddClause(clause);
            AddValues(clause, values);
        }

        public void AddValues(QueryClause clause, List<object> values)
        {
            if (!Clauses.Contains(clause)) { throw new ArgumentException("Clause type has not been added to clause list"); }

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
                    var tBinder = new List<string>();
                    foreach (object item in values)
                    {
                        tBinder.Add(item.ToString());
                    }
                    QueryTables = tBinder;
                    break;
                case QueryClause.Values:
                    var vBinder = new List<string>();
                    foreach (object item in values)
                    {
                        vBinder.Add(item.ToString());
                    }
                    QueryValues = vBinder;
                    break;
                case QueryClause.Where:
                    var wBinder = new List<ComparisonComponent>();
                    foreach (object item in values)
                    {
                        wBinder.Add((ComparisonComponent)item);
                    }
                    QueryWhere = wBinder;
                    break;
                case QueryClause.WhereJoins:
                    var wjBinder = new List<string>();
                    foreach (object item in values)
                    {
                        wjBinder.Add(item.ToString());
                    }
                    QueryWhereJoins = wjBinder;
                    break;
                default:
                    throw new ArgumentException("Invalid clause type provided");
            }
        }

        public string BuildString()
        {
            foreach (var required in _requiredClauses)
            {
                if (!Clauses.Contains(required)) { throw new InvalidOperationException(string.Format("Must provide required clause: {0}", required.ToString())); }
            }

            var queryString = WriteUpdateClause();

            if (Clauses.Contains(QueryClause.Where))
            {
                queryString += string.Format(" WHERE {0}", WriteWhereClause());
            }

            return queryString;
        }

        private string WriteUpdateClause()
        {
            //Confirm that all required clause components are provided and contain values
            var missing = "";
            if (QueryFields is null || QueryFields.Count < 1) { missing = "Fields"; }
            if (QueryTables is null || QueryTables.Count != 1) { if (missing == "") { missing = "Tables"; } else { missing += ", Tables"; } }
            if (QueryValues is null || QueryValues.Count != QueryFields.Count) { if (missing == "") { missing = "Values"; } else { missing += ", Values"; } }

            if (missing != "") { throw new InvalidOperationException(string.Format("Must provide values in the following collections: {0}", missing)); }

            var queryString = string.Format("UPDATE {0} SET {1} = {2}", QueryTables[0], QueryFields[0], QueryValues[0]);

            for (int i = 1; i < QueryFields.Count; i++)
            {
                queryString += string.Format(", {0} = {1}", QueryFields[i], QueryValues[i]);
            }

            return queryString;
        }

        private string WriteWhereClause()
        {
            if (QueryWhere is null || QueryWhere.Count < 1) { throw new InvalidOperationException("No Where clause content provided"); }

            if (QueryWhere.Count > 1 && (QueryWhereJoins is null || QueryWhereJoins.Count < 1)) { throw new InvalidOperationException("Cannot write a where clause with multiple conditions and no joiners."); }

            if (QueryWhere.Count > 1 && (QueryWhereJoins.Count != QueryWhere.Count - 1)) { throw new ArgumentException("Where joins must include one value for every additional where value after the first"); }

            var queryPart = QueryWhere[0].ToString();

            for (int i = 1; i < QueryWhere.Count; i++)
            {
                queryPart += string.Format(" {0} {1}", QueryWhereJoins[i - 1], QueryWhere[i]);
            }

            return queryPart;
        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }

        public void ValidateObjectList(QueryClause target, List<object> objList)
        {
            if (objList is null || objList.Count < 1) { throw new ArgumentException("Must provide an object list containing objects"); }

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
                case QueryClause.Where:
                    if (objList[0].GetType() != typeof(ComparisonComponent)) { valid = false; desiredType = "Comparison Component"; }
                    break;
                case QueryClause.WhereJoins:
                    if (objList[0].GetType() != typeof(string)) { valid = false; desiredType = "string"; }
                    break;
            }

            if (!valid) { throw new ArgumentException(string.Format("{0} clause requires objects of type {1}", target.ToString(), desiredType)); }
        }
    }
}
