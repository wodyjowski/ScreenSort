using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenSort.Algorithms
{
    class ParalellMergeSortAlgorithm : ISortingAlgorithm
    {
        public CancellationToken Token { get; set; }

        public void Sort(int[] ArrayToSort)
        {
            StoParallelMergeSort<int> stM = new StoParallelMergeSort<int>();
            stM.Sort(ArrayToSort);
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


    public class StoParallelMergeSort<T>
    {
        private const int InsertionSortBlockSize = 64;
        private readonly IComparer<T> _comparer;
        private readonly int _maxParallelDepth;
        private bool _ascending = true;

        public StoParallelMergeSort()
        {
            _comparer = Comparer<T>.Default;
            _maxParallelDepth = DetermineMaxParallelDepth();
        }

        public StoParallelMergeSort(Comparison<T> comparison)
        {
            if (comparison == null)
                throw new ArgumentNullException("comparison");
            _comparer = new ComparerFromComparison<T>(comparison);
            _maxParallelDepth = DetermineMaxParallelDepth();
        }

        public StoParallelMergeSort(IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            _comparer = comparer;
            _maxParallelDepth = DetermineMaxParallelDepth();
        }

        public void Sort(T[] list, bool ascending = true)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (list.Length < 2)
                return;
            _ascending = ascending;

            T[] tempList = new T[list.Length];
            SortBlock(list, tempList, 0, list.Length - 1, 1);
        }

        public void Sort(IList<T> list, bool ascending = true)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (list.Count < 2)
                return;

            // Create array from list for fast access
            T[] arrayList = new T[list.Count];
            list.CopyTo(arrayList, 0);

            Sort(arrayList, ascending);

            // Copy ordered elements back to the list
            for (int index = 0; index < arrayList.Length; index++)
                list[index] = arrayList[index];
        }


        protected void SortBlock(T[] list, T[] tempList, int beginBlock, int endBlock, int recursionDepth)
        {
            // Odd levels should store the result in the list, even levels in the
            // in tempList. This swapping avoids array copying from a temp list.
            bool mergeToTempList = recursionDepth % 2 == 0;
            bool workParallel = recursionDepth <= _maxParallelDepth;
            int blockSize = endBlock - beginBlock + 1;
            bool isSmallEnoughForInsertionSort = blockSize <= InsertionSortBlockSize;

            if (isSmallEnoughForInsertionSort)
            {
                // Switch to InsertionSort
                InsertionSort(list, beginBlock, endBlock);
                if (mergeToTempList)
                    Array.Copy(list, beginBlock, tempList, beginBlock, blockSize);
            }
            else
            {
                // Split sorting into halves
                int middle = beginBlock + ((endBlock - beginBlock) / 2); // avoid overflows
                if (workParallel)
                {
                    Parallel.Invoke(
                        () => SortBlock(list, tempList, beginBlock, middle, recursionDepth + 1),
                        () => SortBlock(list, tempList, middle + 1, endBlock, recursionDepth + 1));
                }
                else
                {
                    SortBlock(list, tempList, beginBlock, middle, recursionDepth + 1);
                    SortBlock(list, tempList, middle + 1, endBlock, recursionDepth + 1);
                }

                // Merge sorted halves
                if (mergeToTempList)
                    MergeTwoBlocks(list, tempList, beginBlock, middle, middle + 1, endBlock);
                else
                    MergeTwoBlocks(tempList, list, beginBlock, middle, middle + 1, endBlock);
            }
        }

        protected void MergeTwoBlocks(T[] sourceList, T[] targetList, int beginBlock1, int endBlock1, int beginBlock2, int endBlock2)
        {
            for (int targetIndex = beginBlock1; targetIndex <= endBlock2; targetIndex++)
            {
                if (beginBlock1 > endBlock1)
                {
                    // Nothing is left from block1, take next element from block2
                    targetList[targetIndex] = sourceList[beginBlock2++];
                }
                else if (beginBlock2 > endBlock2)
                {
                    // Nothing is left from block2, take next element from block1
                    targetList[targetIndex] = sourceList[beginBlock1++];
                }
                else
                {
                    // Compare the next elements from both blocks and take the smaller one
                    if (Compare(sourceList[beginBlock1], sourceList[beginBlock2]) <= 0)
                        targetList[targetIndex] = sourceList[beginBlock1++];
                    else
                        targetList[targetIndex] = sourceList[beginBlock2++];
                }
            }
        }

        internal void InsertionSort(T[] list, int beginBlock, int endBlock)
        {
            for (int endAlreadySorted = beginBlock; endAlreadySorted < endBlock; endAlreadySorted++)
            {
                T elementToInsert = list[endAlreadySorted + 1];

                int insertPos = InsertionSortBinarySearch(list, beginBlock, endAlreadySorted, elementToInsert);
                if (insertPos <= endAlreadySorted)
                {
                    // Shift elements to the right to make place for the elementToInsert
                    Array.Copy(list, insertPos, list, insertPos + 1, endAlreadySorted - insertPos + 1);
                    list[insertPos] = elementToInsert;
                }
            }
        }


        internal int InsertionSortBinarySearch(T[] list, int beginBlock, int endBlock, T elementToInsert)
        {
            while (beginBlock <= endBlock)
            {
                int middle = beginBlock + ((endBlock - beginBlock) / 2); // avoid overflows

                int comparisonRes = Compare(elementToInsert, list[middle]);
                if (comparisonRes < 0)
                {
                    // elementToInsert was smaller, go to the left half
                    endBlock = middle - 1;
                }
                else if (comparisonRes > 0)
                {
                    // elementToInsert was bigger, go to the right half
                    beginBlock = middle + 1;
                }
                else
                {
                    // elementToInsert was equal, move to the right as long as elements
                    // are equal, to get the sorting stable
                    beginBlock = middle + 1;
                    while ((beginBlock < endBlock) && (Compare(elementToInsert, list[beginBlock + 1]) == 0))
                        beginBlock++;
                }
            }
            return beginBlock;
        }

        protected int DetermineMaxParallelDepth()
        {
            const int MaxTasksPerProcessor = 8;
            int maxTaskCount = Environment.ProcessorCount * MaxTasksPerProcessor;
            return (int)Math.Log(maxTaskCount, 2);
        }

        protected int Compare(T x, T y)
        {
            int result = _comparer.Compare(x, y);
            if (_ascending)
                return result;
            else
                return -result;
        }

        private class ComparerFromComparison<TU> : IComparer<TU>
        {
            private readonly Comparison<TU> _comparison;

            public ComparerFromComparison(Comparison<TU> comparison)
            {
                _comparison = comparison;
            }

            public int Compare(TU x, TU y)
            {
                return _comparison(x, y);
            }
        }
    }
}
