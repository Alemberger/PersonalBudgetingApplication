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

        public string DateString
        {
            get
            {
                return Date.ToString("MM/dd/yyyy");
            }
        }

        public double Principal { get; set; }

        public double IncreaseAmount { get; set; }

        public string IncreaseDisplay
        {
            get
            {
                string display;

                try
                {
                    display = string.Format("{0:C}", IncreaseAmount);
                }
                catch (FormatException) { return ""; }

                if (display == "$0.00") { return ""; }

                return display;
            }
        }

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
                if (_increaseType.ToString() == "None") { return ""; }

                return _increaseType.ToString();
            }
        }

        public double PaymentMade { get; set; }

        public string PaymentDisplay
        {
            get
            {
                string display;

                try
                {
                    display = string.Format("{0:C}", PaymentMade);
                }
                catch (FormatException) { return ""; }

                if (display == "$0.00") { return ""; }

                return display;
            }
        }

        public DebtOverviewItem()
        {
        }

        public DebtOverviewItem(int debtId)
        {
            DebtID = debtId;
        }
    }
}
