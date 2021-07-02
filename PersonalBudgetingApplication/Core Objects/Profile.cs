using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Core_Objects
{
    public class Profile
    {
        public int ProfileID { get; set; }
        public string Name { get; set; }
        public string RecordBy { get; set; }
        public DateTime? RecordDate { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
