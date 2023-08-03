using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public static class Extensions
    {

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> consumer)
        {
            foreach (T element in enumerable)
                consumer.Invoke(element);
            
        }

        public static Vector3 Add(this Vector3 start, Vector3 destination)
        {
            return new Vector3(start.x + destination.x, start.y + destination.y, start.y + destination.y);
        }
    }
}