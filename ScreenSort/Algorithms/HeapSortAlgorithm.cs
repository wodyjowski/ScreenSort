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
        public int Delay { get; set; }

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
                Task.Delay(Delay);
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

        public void Sort(int[] ArrayToSort, HSBColor[] FloatArrayToSort)
        {
            heapSort(ArrayToSort, ArrayToSort.Length, FloatArrayToSort);
        }


        void heapSort(int[] arr, int n, HSBColor[] FloatArrayToSort)
        {
            for (int i = n / 2 - 1; i >= 0 && !Token.IsCancellationRequested; i--)
                heapify(arr, n, i, FloatArrayToSort);
            for (int i = n - 1; i >= 0 && !Token.IsCancellationRequested; i--)
            {
                Task.Delay(Delay);
                var temp = FloatArrayToSort[0];
                FloatArrayToSort[0] = FloatArrayToSort[i];
                FloatArrayToSort[i] = temp;

                var temp2 = arr[0];
                arr[0] = arr[i];
                arr[i] = temp2;


                heapify(arr, i, 0, FloatArrayToSort);
            }
        }
        void heapify(int[] arr, int n, int i, HSBColor[] FloatArrayToSort)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            if (left < n && FloatArrayToSort[left] > FloatArrayToSort[largest])
                largest = left;
            if (right < n && FloatArrayToSort[right] > FloatArrayToSort[largest])
                largest = right;
            if (largest != i)
            {
                var swap = FloatArrayToSort[i];
                FloatArrayToSort[i] = FloatArrayToSort[largest];
                FloatArrayToSort[largest] = swap;

                var swap2 = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap2;

                heapify(arr, n, largest, FloatArrayToSort);
            }
        }
    }
}
