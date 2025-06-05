using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.limphus.utilities
{
    //sources from here - https://discussions.unity.com/t/proper-way-to-shift-elements-in-int-array/579943/5
    public static class ShiftArray
    {
        public static int[] ShiftIntArray(this int[] myArray)
        {
            int[] tArray = new int[myArray.Length];
            int v = myArray[0];
            Array.Copy(myArray, 1, tArray, 0, myArray.Length - 1);
            tArray[tArray.Length - 1] = v;
            return tArray;
        }

        public static GameObject[] ShiftGOArray(this GameObject[] myArray)
        {
            GameObject[] tArray = new GameObject[myArray.Length];
            GameObject v = myArray[0];
            Array.Copy(myArray, 1, tArray, 0, myArray.Length - 1);
            tArray[tArray.Length - 1] = v;
            return tArray;
        }
    }
}