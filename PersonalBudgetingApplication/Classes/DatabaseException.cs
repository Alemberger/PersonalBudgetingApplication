using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class DatabaseException : Exception
    {
        public string ErrorMessage { get; set; }

        public DatabaseException(string message)
        {
            ErrorMessage = message;
        }
    }
}
