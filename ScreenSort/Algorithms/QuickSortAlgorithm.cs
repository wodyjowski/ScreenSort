using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WBAnimation.Algorithms
{
    class QuickSortAlgorithm : ISortingAlgorithm
    {
        public CancellationToken Token { get; set; }

        public void Sort(int[] ArrayToSort)
        {
            QuickSort(ArrayToSort, 0, ArrayToSort.Length - 1, Token);
        }

        private void QuickSort(int[] array, int left, int right, CancellationToken cToken)
        {
            if (cToken.IsCancellationRequested)
            {
                return;
            }
            var i = left;
            var j = right;
            var pivot = array[(left + right) / 2];
            while (i < j)
            {
                Task.Delay(TimeSpan.FromMilliseconds(0.84));

                while (array[i] < pivot) i++;
                while (array[j] > pivot) j--;
                if (i <= j)
                {
                    // swap
                    var tmp = array[i];
                    array[i++] = array[j];
                    array[j--] = tmp;
                }
            }
            if (left < j) QuickSort(array, left, j, cToken);
            if (i < right) QuickSort(array, i, right, cToken);
        }
    }
}
