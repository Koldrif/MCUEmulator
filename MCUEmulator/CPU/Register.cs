using System.Collections;
using System.Text;

namespace MCUEmulator;

public class Register
{
    private BitArray _bits;
    public Register(int size = 8)
    {
        _bits = new(size);
    }

    public bool GetBit(int index)
    {
        return _bits[index];
    }

    public void SetBit(int index, bool value)
    {
        _bits[index] = value;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append($"[");
        foreach (bool bit in _bits)
        {
            builder.Append($" {(bit ? 1 : 0)} ");
        }

        builder.Append("]");
        return builder.ToString();
    }

    public void SetRegisterValue(BitArray value)
    {
        _bits = value;
    }

    public void SetRegisterValue(int value)
    {
        if (value > Math.Pow(2, _bits.Count))
            throw new ArgumentException($"To big value for {_bits.Count} bits");
        // _bits = new BitArray(value);
        var tmp = fromDecToBin(value);
        _bits = tmp.resizeUpTo(_bits.Count);

    }

    private BitArray fromDecToBin(int value)
    {
        List<bool> result = new List<bool>();
        while (value > 0)
        {
            int m = value % 2;
            value = value / 2;
            result.Add(m == 1);
        }
        result.Reverse();
        return new BitArray(result.ToArray());
    }
    
    
    /*
     * TODO
     *  1. Сдвиг цикличный, обычный
     */
}

public static class BitArrayExtension
{
    // Have 0010 need extend upto 8 bits
    public static BitArray resizeUpTo(this BitArray array, int to)
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
}