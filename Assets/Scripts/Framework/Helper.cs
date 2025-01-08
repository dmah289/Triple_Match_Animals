using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class Helper
    {
        public static class NonAllocatingWait
        {
            public static WaitForEndOfFrame WaitForEndOfFrameNonAllocating { get; } = new();
            
            public static Dictionary<float, WaitForSeconds> WaitDict { get; } = new();

            public static WaitForSeconds GetWait(float seconds)
            {
                if (WaitDict.TryGetValue(seconds, out var result)) return result;
            
                WaitDict[seconds] = new(seconds);
                return WaitDict[seconds];
            }
        }
    }
}