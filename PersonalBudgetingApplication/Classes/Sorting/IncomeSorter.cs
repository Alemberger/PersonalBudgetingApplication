using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetingApplication.Classes.Sorting
{
    class IncomeSorter : Sorter
    {
        public List<Income> Parent { get; set; }

        private IncomeSorter() { }

        public IncomeSorter(List<Income> parent)
        {
            Parent = parent;
        }

        public List<Income> SortByDateDescending()
        {
            var sorted = new List<Income>(Parent.Count);

            var newIndexes = new int[Parent.Count];

            for (int i = 0; i < newIndexes.Length; i++)
            {
                newIndexes[i] = -1;
            }

            newIndexes[0] = 0;

            for (int i = 1; i < Parent.Count; i++)
            {
                var currentIndex = i;

                var record = Parent[currentIndex];

                var greaterThan = false;

                var checkIndex = 0;

                while (newIndexes[checkIndex] != -1)
                {
                    if (record.Date > Parent[newIndexes[checkIndex]].Date)
                    {
                        greaterThan = true;
                    }

                    if (greaterThan)
                    {
                        break;
                    }

                    checkIndex++;
                }

                if (checkIndex == currentIndex) { newIndexes[currentIndex] = currentIndex; }
                else
                {
                    int[] transfer = new int[currentIndex + 1];

                    for (int j = 0; j < checkIndex; j++)
                    {
                        transfer[j] = newIndexes[j];
                    }

                    transfer[checkIndex] = currentIndex;

                    for (int j = checkIndex + 1; j <= currentIndex; j++)
                    {
                        transfer[j] = newIndexes[j - 1];
                    }

                    for (int j = 0; j <= currentIndex; j++)
                    {
                        newIndexes[j] = transfer[j];
                    }
                }
            }

            for (int i = 0; i < Parent.Count; i++)
            {
                sorted.Add(Parent[newIndexes[i]].Transfer());
            }

            Parent = sorted;

            return sorted;
        }

        public List<Income> SortByDateAscending()
        {
            var sorted = SortByDateDescending();

            var reversed = new List<Income>();

            for (int i = sorted.Count - 1; i >= 0; i--)
            {
                reversed.Add(sorted[i].Transfer());
            }

            Parent = reversed;

            return reversed;
        }
    }
}
