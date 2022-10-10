using System.Collections;

namespace MCUEmulator.Utility;

public static class BitArrayExtension
{
    // Have 0010 need extend upto 8 bits
    /// <summary>
    /// Resizes register
    /// </summary>
    /// <param name="to">Expands register to param value</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">You can't decrease size of register</exception>
    public static BitArray ResizeUpTo(this BitArray array, int to)
    {
        var oldArraySize = array.Count;
        if (oldArraySize > to)
            throw new ArgumentException($"You can't extend array with size {oldArraySize}, to lower count {to} bits");
        var newArray = new BitArray(to, false);
        for (int i = 0; i < oldArraySize; i++)
        {
            newArray[^(i + 1)] = array[^(i + 1)];
        }

        return newArray;
    }

    public static int ToInt(this BitArray bitArray)
    {
        bitArray.Reverse();
        if (bitArray.Length > 32)
            throw new ArgumentException("Argument length shall be at most 32 bits.");

        int value = 0;

        for (int i = 0; i < bitArray.Count; i++)
        {
            if (bitArray[i])
                value += Convert.ToInt32(Math.Pow(2, i));
        }
        bitArray.Reverse();
        return value;
    }
    
    public static void Reverse(this BitArray array)
    {
        var length = array.Length;
        var mid = (length / 2);

        for (int i = 0; i < mid; i++)
        {
            (array[i], array[length - i - 1]) = (array[length - i - 1], array[i]);
        }    
    }
}