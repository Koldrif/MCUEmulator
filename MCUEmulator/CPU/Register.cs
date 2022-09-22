using System.Collections;
using System.Text;

namespace MCUEmulator;

public class Register
{
    private BitArray bits;
    public Register(int size = 8)
    {
        bits = new(size);
    }

    public bool getBit(int index)
    {
        return bits[index];
    }

    public void setBit(int index, bool value)
    {
        bits[index] = value;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append($"[");
        foreach (bool bit in bits)
        {
            builder.Append($" {(bit ? 1 : 0)} ");
        }

        builder.Append("]");
        return builder.ToString();
    }
    
    /*
     * TODO
     *  1. Сдвиг цикличный, обычный
     */
}