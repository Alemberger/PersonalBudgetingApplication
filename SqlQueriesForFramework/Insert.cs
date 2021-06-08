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
        public ExecutionType ExecType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string QueryString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataSet QueryResults { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}
