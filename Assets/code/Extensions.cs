﻿using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Common;

namespace code
{
    public static class Extensions
    {

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> consumer)
        {
            foreach (T element in enumerable)
                consumer.Invoke(element);
            
        }
    }
}