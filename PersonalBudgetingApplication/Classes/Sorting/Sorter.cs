using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes.Sorting
{
    class Sorter
    {
        public Sorter() { }

        /// <summary>
        /// Compares two dates and returns an integer value based on the date that is larger
        /// </summary>
        /// <param name="a">First date</param>
        /// <param name="b">Second date</param>
        /// <returns>0 = dates are equal, -1 = a is later, 1 = b is later</returns>
        /// <exception cref="Exception"></exception>
        public int SortDates(DateTime a, DateTime b)
        {
            if (a.Date == b.Date)
            {
                return 0;
            }
            else if (a.Date > b.Date)
            {
                return -1;
            }
            else if (a.Date < b.Date)
            {
                return 1;
            }
            else
            {
                throw new Exception("Invalid dates provided");
            }
        }
    }
}
