using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenSort.Algorithms
{
    class BubbleSortAlgorithm : ISortingAlgorithm
    {
        public CancellationToken Token { get; set; }

        public void Sort(int[] ArrayToSort)
        {
            BubbleSort(ArrayToSort);
        }

        public void BubbleSort(int[] array)
        {
            int temp = 0;

            for (int write = 0; write < array.Length; write++)
            {
                for (int sort = 0; sort < array.Length - 1; sort++)
                {
                    if(Token.IsCancellationRequested)
                    {
                        return;
                    }

                    if (array[sort] > array[sort + 1])
                    {
                        temp = array[sort + 1];
                        array[sort + 1] = array[sort];
                        array[sort] = temp;
                    }
                }
            }

        }

        public void Sort(int[] ArrayToSort, HSBColor[] FloatArrayToSort)
        {
            BubbleSort(ArrayToSort, FloatArrayToSort);
        }

        public void BubbleSort(int[] array, HSBColor[] FloatArrayToSort)
        {
            for (int write = 0; write < FloatArrayToSort.Length; write++)
            {
                for (int sort = 0; sort < FloatArrayToSort.Length - 1; sort++)
                {
                    if (Token.IsCancellationRequested)
                    {
                        return;
                    }

                    if (FloatArrayToSort[sort] > FloatArrayToSort[sort + 1])
                    {
                        var temp = FloatArrayToSort[sort + 1];
                        FloatArrayToSort[sort + 1] = FloatArrayToSort[sort];
                        FloatArrayToSort[sort] = temp;


                        var temp2 = array[sort + 1];
                        array[sort + 1] = array[sort];
                        array[sort] = temp2;

                    }
                }
            }

        }
    }
}
