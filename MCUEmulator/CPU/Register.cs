using System.Collections;
using System.Text;

namespace MCUEmulator.CPU;
/// <summary>
/// Class that represents register in CPU with methods that can operate triggers inside 
/// </summary>
public class Register
{
    private BitArray _bits;
    /// <summary>
    /// Returns <see cref="BitArray"/> of current <see cref="Register"/> 
    /// </summary>
    public BitArray Bits => _bits;

    public Register(int size = 8)
    {
        _bits = new(size);
    }

    /// <summary>
    /// Returns bit value of register
    /// </summary>
    /// <param name="index">Index of bit, that you want to get</param>
    /// <returns>Bit value</returns>
    public bool GetBit(int index)
    {
        return _bits[index];
    }

    /// <summary>
    /// Sets bit value under index
    /// </summary>
    /// <param name="index">Index of bit you want to change</param>
    /// <param name="value"><see cref="bool"/> value you want to set bit to</param>
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
    /// <summary>
    /// Sets register values, equals to binary analogue of input parameter
    /// </summary>
    /// <param name="value">value, that you want to put into <see cref="Register"/></param>
    /// <exception cref="ArgumentException">if value bigger then this register size. E.G. if you want to set value
    /// 16 into register with size 3
    /// </exception>
    public void SetRegisterValue(int value)
    {
        if (value >= Math.Pow(2, _bits.Count))
            throw new ArgumentException($"To big value for {_bits.Count} bits");
        // _bits = new BitArray(value);
        var tmp = FromDecToBin(value);
        _bits = tmp.ResizeUpTo(_bits.Count);

    }

    /// <summary>
    /// Converts decimal value to binary
    /// </summary>
    /// <param name="value">Decimal value that would be converted into binary <see cref="BitArray"/></param>
    /// <returns><see cref="BitArray"/> with values </returns>
    private static BitArray FromDecToBin(int value)
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
    /// <summary>
    /// Shifts register in a one of two directions <see cref="ShiftDirection"/>
    /// </summary>
    /// <param name="direction">Accepts two directions Left or Right. <see cref="ShiftDirection"/></param>
    /// <returns>The value of moved bit</returns>
    /// <exception cref="ArgumentException">If peeked unknown direction</exception>
    public bool Shift(ShiftDirection direction)
    {
        bool tmpTrigger = false;
        switch (direction)
        {
            case ShiftDirection.Left:
                return Left();
            case ShiftDirection.Right:
                return Right();
            default:
                throw new ArgumentException("Unknown shift direction");
        }
        return tmpTrigger;
        bool Left()
        {
            tmpTrigger = _bits[^1];
            for (int i = _bits.Count - 1; i >= 1; i--)
            {
                _bits[i] = _bits[i - 1];
            }

            _bits[0] = false;
            return tmpTrigger;
        }        
        bool Right()
        {
            tmpTrigger = _bits[0];
            for (int i = 0; i < _bits.Count - 1; i++)
            {
                _bits[i] = _bits[i + 1];
            }

            _bits[^1] = false;
            return tmpTrigger;
        }
        
        
        
    }
    
    /// <summary>
    /// Enum for left and right directions
    /// </summary>
    public enum ShiftDirection
    {
        Left,
        Right
    }
}

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
}