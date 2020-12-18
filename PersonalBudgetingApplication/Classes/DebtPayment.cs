using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    public class DebtPayment
    {
        public int ID { get; set; }

        public int DebtID { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public string RecordBy { get; set; }

        public DateTime RecordDate { get; set; }

        public DebtPayment() { }
    }
}
