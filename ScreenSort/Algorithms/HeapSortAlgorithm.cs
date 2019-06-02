using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenSort.Algorithms
{
    class HeapSortAlgorithm : ISortingAlgorithm
    {
        public CancellationToken Token { get; set; }

        public void Sort(int[] ArrayToSort)
        {
            heapSort(ArrayToSort, ArrayToSort.Length);
        }

        void heapSort(int[] arr, int n)
        {
            for (int i = n / 2 - 1; i >= 0 && !Token.IsCancellationRequested; i--)
                heapify(arr, n, i);
            for (int i = n - 1; i >= 0 && !Token.IsCancellationRequested; i--)
            {
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;
                heapify(arr, i, 0);
            }
        }
        void heapify(int[] arr, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            if (left < n && arr[left] > arr[largest])
                largest = left;
            if (right < n && arr[right] > arr[largest])
                largest = right;
            if (largest != i)
            {
                int swap = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap;
                heapify(arr, n, largest);
            }
        }
    }
}
