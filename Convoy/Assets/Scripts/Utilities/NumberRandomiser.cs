using UnityEngine;

namespace com.limphus.utilities
{
    public class NumberRandomiser
    {
        public static float[] GetRandomFloats(float min, float max, int amount)
        {
            if (amount == 0) return null;

            float[] floats = new float[amount];

            for (int i = 0; i < floats.Length; i++)
            {
                floats[i] = Random.Range(min, max);
            }

            return floats;
        }

        public static int[] GetRandomInts(int min, int max, int amount)
        {
            if (amount == 0) return null;

            int[] ints = new int[amount];

            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = Random.Range(min, max);
            }

            return ints;
        }

        public static float GetRandomFloat(float min, float max)
        {
            return Random.Range(min, max);
        }

        public static int GetRandomInt(int min, int max)
        {
            return Random.Range(min, max);
        }
    }
}