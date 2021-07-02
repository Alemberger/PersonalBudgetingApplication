using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Core_Objects
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public double Amount { get; set; }
        public DateTime? Date { get; set; }
        public TransactionType Type { get; set; }
        public int Category { get; set; }
        public string RecordBy { get; set; }
        public DateTime? RecordDate { get; set; }

        public Account Account { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }

    public enum TransactionType
    {
        Income = 1,
        Expense
    }
}
