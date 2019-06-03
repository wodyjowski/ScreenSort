using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenSort.Algorithms
{
    class ParallelQuickSortAlgorithm : ISortingAlgorithm
    {
        public CancellationToken Token { get; set; }

        public void Sort(int[] ArrayToSort)
        {
            QuickSort(ArrayToSort, 0, ArrayToSort.Length - 1);
        }

        public static void swap<T>(T[] arr, int i, int j)
        {
            // Swap two element in an array with given indexes.
            var temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        private static int partition<T>(T[] arr, int lo, int hi)
          where T : IComparable<T>
        {
            /* 
            ** Partition an array according to a selected pivot, return the index of the pivot.
            ** Result: Values of elements before pivot are less than the pivot, 
            ** Values of elements after pivot are greater than or equals to the pivot
            */
            int j = lo;
            var pivot = arr[lo];
            for (int i = lo; i <= hi; i++)
            {
                if (arr[i].CompareTo(pivot) >= 0)
                {
                    continue;
                }
                j++;
                swap(arr, i, j);
            }
            swap(arr, lo, j);
            return j;
        }

        private static void InsertionSort<T>(T[] arr, int lo, int hi)
          where T : IComparable<T>
        {
            /* To deal with small arrays
            ** Loop through the array from the second element, insert the element to the correct position
            ** 
            */
            for (int i = lo + 1; i <= hi; i++)
            {

                int j = i - 1;
                var x = arr[i];
                while (j >= lo && arr[j].CompareTo(x) > 0)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = x;
            }
        }

        public static void QuickSort<T>(T[] arr, int lo, int hi)
          where T : IComparable<T>
        {
            /*
            ** QuickSort an array, if the array length is less than 40, use InsertionSort instead. 
            ** 
            */
            if (hi - lo < 40)
            {
                InsertionSort(arr, lo, hi);
            }
            else
            {
                int p = partition(arr, lo, hi);
                QuickSort(arr, lo, p - 1);
                QuickSort(arr, p + 1, hi);
            }
        }


        public static void QuickSortParallel<T>(T[] arr, int lo, int hi)
          where T : IComparable<T>
        {
            if (hi - lo < 2000)
            {
                QuickSort(arr, lo, hi);
            }
            else
            {
                int p = partition(arr, lo, hi);
                Parallel.Invoke(
                  () => QuickSortParallel(arr, lo, p - 1),
                  () => QuickSortParallel(arr, p + 1, hi)
                );
            }
        }

        public void Sort(int[] ArrayToSort, float[] FloatArrayToSort)
        {
            throw new NotImplementedException();
        }

        public void Sort(int[] ArrayToSort, ref float[] FloatArrayToSort)
        {
            throw new NotImplementedException();
        }

        public void Sort(int[] ArrayToSort, int[] FloatArrayToSort)
        {
            throw new NotImplementedException();
        }

        public void Sort(int[] ArrayToSort, HSBColor[] FloatArrayToSort)
        {
            throw new NotImplementedException();
        }
    }
}
