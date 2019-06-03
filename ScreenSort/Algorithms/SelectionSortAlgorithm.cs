using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenSort.Algorithms
{
    class SelectionSortAlgorithm : ISortingAlgorithm
    {
        public CancellationToken Token { get; set; }
        public int Delay { get; set; }

        public void Sort(int[] ArrayToSort)
        {
            IntArraySelectionSort(ArrayToSort);
        }

        public int IntArrayMin(int[] data, int start)
        {
            int minPos = start;
            for (int pos = start + 1; pos < data.Length; pos++)
                if (data[pos] < data[minPos])
                    minPos = pos;
            return minPos;
        }

        public void IntArraySelectionSort(int[] data)
        {
            int i;
            int N = data.Length;

            for (i = 0; i < N - 1; i++)
            {
                if(Token.IsCancellationRequested)
                {
                    return;
                }

                int k = IntArrayMin(data, i);
                if (i != k)
                {
                    int temporary;
                    temporary = data[i];
                    data[i] = data[k];
                    data[k] = temporary;
                }
            }
        }

        public void Sort(int[] ArrayToSort, HSBColor[] FloatArrayToSort)
        {
            IntArraySelectionSort(ArrayToSort, FloatArrayToSort);
        }

        public int IntArrayMin(int[] data, int start, HSBColor[] FloatArrayToSort)
        {
            int minPos = start;
            for (int pos = start + 1; pos < FloatArrayToSort.Length; pos++)
                if (FloatArrayToSort[pos] < FloatArrayToSort[minPos])
                    minPos = pos;
            return minPos;
        }

        public void IntArraySelectionSort(int[] data, HSBColor[] FloatArrayToSort)
        {
            int i;
            int N = FloatArrayToSort.Length;

            for (i = 0; i < N - 1; i++)
            {
                if (Token.IsCancellationRequested)
                {
                    return;
                }

                int k = IntArrayMin(data, i, FloatArrayToSort);
                if (i != k)
                {
                    HSBColor temporary;
                    temporary = FloatArrayToSort[i];
                    FloatArrayToSort[i] = FloatArrayToSort[k];
                    FloatArrayToSort[k] = temporary;

                    int temporary1;
                    temporary1 = data[i];
                    data[i] = data[k];
                    data[k] = temporary1;
                }
            }
        }
    }
}
