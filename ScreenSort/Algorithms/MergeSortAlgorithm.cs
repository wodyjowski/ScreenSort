using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenSort.Algorithms
{
    class MergeSortAlgorithm : ISortingAlgorithm
    {
        public CancellationToken Token { get; set; }
        public int Delay { get; set; }

        public void Sort(int[] ArrayToSort)
        {
            mergeSort(ArrayToSort, 0, ArrayToSort.Length - 1);
        }

        public void merge(int[] arr, int p, int q, int r)
        {
            int i, j, k;
            int n1 = q - p + 1;
            int n2 = r - q;
            int[] L = new int[n1];
            int[] R = new int[n2];
            for (i = 0; i < n1; i++)
            {
                L[i] = arr[p + i];
            }
            for (j = 0; j < n2; j++)
            {
                R[j] = arr[q + 1 + j];
            }
            i = 0;
            j = 0;
            k = p;
            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                {
                    arr[k] = L[i];
                    i++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                }
                k++;
            }
            while (i < n1)
            {
                arr[k] = L[i];
                i++;
                k++;
            }
            while (j < n2)
            {
                arr[k] = R[j];
                j++;
                k++;
            }
        }
        public void mergeSort(int[] arr, int p, int r)
        {
            if (Token.IsCancellationRequested)
            {
                return;
            }
            if (p < r)
            {
                Task.Delay(Delay);
                int q = (p + r) / 2;
                mergeSort(arr, p, q);
                mergeSort(arr, q + 1, r);
                merge(arr, p, q, r);
            }
        }


        public void Sort(int[] ArrayToSort, HSBColor[] FloatArrayToSort)
        {
            mergeSort(ArrayToSort, 0, ArrayToSort.Length - 1, FloatArrayToSort);
        }


        public void merge(int[] arr, int p, int q, int r, HSBColor[] FloatArrayToSort)
        {
            int i, j, k;
            int n1 = q - p + 1;
            int n2 = r - q;
            HSBColor[] L = new HSBColor[n1];
            HSBColor[] R = new HSBColor[n2];

            int[] Li = new int[n1];
            int[] Ri = new int[n2];

            for (i = 0; i < n1; i++)
            {
                L[i] = FloatArrayToSort[p + i];
                Li[i] = arr[p + i];
            }
            for (j = 0; j < n2; j++)
            {
                R[j] = FloatArrayToSort[q + 1 + j];
                Ri[j] = arr[q + 1 + j];
            }
            i = 0;
            j = 0;
            k = p;
            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                {
                    FloatArrayToSort[k] = L[i];
                    arr[k] = Li[i];
                    i++;
                }
                else
                {
                    FloatArrayToSort[k] = R[j];
                    arr[k] = Ri[j];
                    j++;
                }
                k++;
            }
            while (i < n1)
            {
                FloatArrayToSort[k] = L[i];
                arr[k] = Li[i];
                i++;
                k++;
            }
            while (j < n2)
            {
                FloatArrayToSort[k] = R[j];
                arr[k] = Ri[j];
                j++;
                k++;
            }
        }
        public void mergeSort(int[] arr, int p, int r, HSBColor[] FloatArrayToSort)
        {
            if (Token.IsCancellationRequested)
            {
                return;
            }
            if (p < r)
            {
                Task.Delay(Delay);
                int q = (p + r) / 2;
                mergeSort(arr, p, q, FloatArrayToSort);
                mergeSort(arr, q + 1, r, FloatArrayToSort);
                merge(arr, p, q, r, FloatArrayToSort);
            }
        }

    }
}
