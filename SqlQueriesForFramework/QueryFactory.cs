using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlQueriesForFramework
{
    public class QueryFactory
    {
        public QueryType TargetQuery { get; set; } = QueryType.Unset;

        List<QueryClause> IncludedClauses { get; set; }
        Dictionary<QueryClause, List<object>> ClauseValues { get; set; }

        public void IncludeClause(QueryClause clause)
        {
            if (!IncludedClauses.Contains(clause))
            {
                IncludedClauses.Add(clause);
            }
            else { throw new ArgumentException("This clause is already added"); }
        }

        public string ListIncludedClauses()
        {
            var output = "Included Clauses: ";

            if (IncludedClauses.Count < 1) { return output += "None"; }

            output += IncludedClauses[0].ToString();

            for (int i = 1; i < IncludedClauses.Count; i++)
            {
                output += string.Format(", {0}", IncludedClauses[i].ToString());
            }

            return output;
        }

        public void AddValues(QueryClause clause, List<object> content)
        {
            
        }

        public IQuery BuildQuery(QueryType type)
        {
            if (TargetQuery == QueryType.Unset)
            {
                throw new ArgumentException("Must select a query type to create");
            }

            if (IncludedClauses is null || IncludedClauses.Count < 1) { throw new InvalidOperationException("Must include clauses for the query"); }

            if (!ValidateIncludedClauses())
            {
                throw new ArgumentException("Some included clauses are not valid for the chosen query type");
            }

            if (!CheckRequiredClauses())
            {
                throw new ArgumentException("Not all required clauses are provided");
            }

            switch (type)
            {
                case QueryType.Select:
                    return new Select();
                case QueryType.Insert:
                    return new Insert();
                case QueryType.Update:
                    return new Update();
                case QueryType.Delete:
                    return new Delete();
            }

            throw new ArgumentException();
        }

        private bool ValidateIncludedClauses()
        {
            var success = true;
            foreach(var check in IncludedClauses)
            {
                switch (TargetQuery)
                {
                    case QueryType.Select:
                        success = Select.ValidClause(check);
                        break;
                    case QueryType.Insert:
                        success = Insert.ValidClause(check);
                        break;
                    case QueryType.Update:
                        success = Insert.ValidClause(check);
                        break;
                    case QueryType.Delete:
                        success = Insert.ValidClause(check);
                        break;
                    default:
                        throw new InvalidOperationException("No valid query type provided to the factory");
                }

                if (!success) { break; }
            }

            return success;
        }

        public bool CheckRequiredClauses()
        {
            var success = true;
            switch (TargetQuery)
            {
                case QueryType.Select:
                    success = Select.CheckForRequired(IncludedClauses.ToArray());
                    break;
                case QueryType.Insert:
                    success = Insert.CheckForRequired(IncludedClauses.ToArray());
                    break;
                case QueryType.Update:
                    success = Update.CheckForRequired(IncludedClauses.ToArray());
                    break;
                case QueryType.Delete:
                    success = Delete.CheckForRequired(IncludedClauses.ToArray());
                    break;
            }

            return success;
        }
    }
}
