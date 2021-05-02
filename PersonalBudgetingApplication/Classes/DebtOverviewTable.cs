using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes
{
    class DebtOverviewTable
    {
        private List<DebtOverviewItem> _items;

        public int DebtID { get; set; }

        public List<DebtOverviewItem> Items { get { return _items; } }

        private DebtOverviewTable() { }

        public DebtOverviewTable(Debt debt)
        {
            DebtID = debt.ID;

            var list = new List<DebtOverviewItem>();

            var earliestDate = debt.GetEarliestDate();

            var dateRange = debt.CalculateDateRange();

            for (int i = 0; i <= dateRange; i++)
            {
                var date = earliestDate.AddDays(i);

                var dateIncreases = debt.GetDebtIncreasesForDate(date);
                var datePayments = debt.GetDebtPaymentsForDate(date);

                var paymentsCount = 0;
                var increasesCount = 0;

                for (int j = 0; j < dateIncreases.Count && j < datePayments.Count; j++)
                {
                    var item = new DebtOverviewItem(DebtID)
                    {
                        Date = date,
                        Principal = AdjustPrincipal(list, debt.Principal),

                        IncreaseAmount = dateIncreases[j].Amount,
                        IncreaseType = dateIncreases[j].IncreaseType,

                        PaymentMade = datePayments[j].Amount
                    };

                    paymentsCount++;
                    increasesCount++;

                    list.Add(item);
                }

                if (dateIncreases.Count > datePayments.Count)
                {
                    for(int j = datePayments.Count; j < dateIncreases.Count; j++)
                    {
                        var item = new DebtOverviewItem(DebtID)
                        {
                            Date = date,
                            Principal = AdjustPrincipal(list, debt.Principal),

                            IncreaseAmount = dateIncreases[j].Amount,
                            IncreaseType = dateIncreases[j].IncreaseType
                        };

                        list.Add(item);
                    }
                }
                else if (datePayments.Count > dateIncreases.Count)
                {
                    for(int j = dateIncreases.Count; j < datePayments.Count; j++)
                    {
                        var item = new DebtOverviewItem(DebtID)
                        {
                            Date = date,
                            Principal = AdjustPrincipal(list, debt.Principal),

                            PaymentMade = datePayments[j].Amount
                        };

                        list.Add(item);
                    }
                }

                if (dateIncreases.Count == 0 && datePayments.Count == 0)
                {
                    var item = new DebtOverviewItem(DebtID)
                    {
                        Principal = AdjustPrincipal(list, debt.Principal),
                        Date = date
                    };

                    list.Add(item);
                }
            }

            var finalItem = new DebtOverviewItem(DebtID)
            {
                Principal = AdjustPrincipal(list, debt.Principal),
                Date = DateTime.Now
            };

            list.Add(finalItem);

            //Reverse Order
            var reversed = new List<DebtOverviewItem>();

            for(int i = list.Count - 1; i >= 0; i--)
            {
                reversed.Add(list[i].Transfer());
            }

            _items = reversed;
        }

        public double AdjustPrincipal(List<DebtOverviewItem> list, double startingPrincipal)
        {
            var difference = 0.00;

            foreach (var item in list)
            {
                if (item.IncreaseAmount > 0.00)
                {
                    difference += item.IncreaseAmount;
                }

                if (item.PaymentMade > 0.00)
                {
                    difference -= item.PaymentMade;
                }
            }

            return startingPrincipal + difference;
        }
    }
}
