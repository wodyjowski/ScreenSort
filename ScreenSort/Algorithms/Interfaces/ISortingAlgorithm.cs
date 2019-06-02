﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WBAnimation.Algorithms
{
    interface ISortingAlgorithm
    {
        CancellationToken Token { get; set; }

        void Sort(int[] ArrayToSort);
    }
}