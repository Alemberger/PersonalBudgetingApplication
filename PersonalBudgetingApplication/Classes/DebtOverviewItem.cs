using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class DebtOverviewItem
    {
        private DebtIncreaseType _increaseType;

        public int DebtID { get; set; }

        public DateTime Date { get; set; }

        public double Principal { get; set; }

        public double IncreaseAmount { get; set; }

        public DebtIncreaseType IncreaseType
        {
            get
            {
                return _increaseType;
            }

            set
            {
                _increaseType = value;
            }
        }

        public string IncreaseTypeName
        {
            get
            {
                return _increaseType.ToString();
            }
        }

        public double PaymentMade { get; set; }

        public DebtOverviewItem()
        {
        }

        public DebtOverviewItem(int debtId)
        {
            DebtID = debtId;
        }
    }
}
