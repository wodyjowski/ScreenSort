using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenSort.Algorithms
{
    class QuickSortAlgorithm : ISortingAlgorithm
    {
        public CancellationToken Token { get; set; }
        public int Delay { get; set; }

        public void Sort(int[] ArrayToSort)
        {
            QuickSort(ArrayToSort, 0, ArrayToSort.Length - 1);
        }

        public void Sort(int[] ArrayToSort, HSBColor[] FloatArrayToSort)
        {
            QuickSort(ArrayToSort, 0, ArrayToSort.Length - 1, FloatArrayToSort);
        }


        private void QuickSort(int[] array, int left, int right, HSBColor[] FloatArrayToSort)
        {
            var i = left;
            var j = right;
            var pivot = FloatArrayToSort[(left + right) / 2];
            while (i < j)
            {
                if (Token.IsCancellationRequested)
                {
                    return;
                }
                while (FloatArrayToSort[i] < pivot) i++;
                while (FloatArrayToSort[j] > pivot) j--;
                if (i <= j)
                {
                    // swap
                    var tmp = FloatArrayToSort[i];
                    FloatArrayToSort[i] = FloatArrayToSort[j];
                    FloatArrayToSort[j] = tmp;

                    var tmp2 = array[i];
                    array[i++] = array[j];
                    array[j--] = tmp2;
                }
            }
            Task.Delay(Delay);
            if (left < j) QuickSort(array, left, j, FloatArrayToSort);
            if (i < right) QuickSort(array, i, right, FloatArrayToSort);
        }



        private void QuickSort(int[] array, int left, int right)
        {
            var i = left;
            var j = right;
            var pivot = array[(left + right) / 2];
            while (i < j)
            {
                if (Token.IsCancellationRequested)
                {
                    return;
                }
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
            Task.Delay(Delay);
            if (left < j) QuickSort(array, left, j);
            if (i < right) QuickSort(array, i, right);
        }
    }
}