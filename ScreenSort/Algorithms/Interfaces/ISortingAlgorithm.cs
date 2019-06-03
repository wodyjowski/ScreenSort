using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenSort.Algorithms
{
    interface ISortingAlgorithm
    {
        CancellationToken Token { get; set; }
        int Delay { get; set; }

        void Sort(int[] ArrayToSort);
        void Sort(int[] ArrayToSort, HSBColor[] FloatArrayToSort);
    }
}
