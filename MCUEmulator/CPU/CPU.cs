using System.Collections;

namespace MCUEmulator;


public class Cpu
{
    private const int InstructionMemorySize = 34;
    
    private const int RegisterSize = 8;

    private Random _rnd = new Random((int)DateTime.Now.Ticks);
    // Instruction Memory
    private List<Register> _instractionsMemory = new(InstructionMemorySize);
    // Data Memory
    private List<Register> _dataMemory = new(InstructionMemorySize);

    private Dictionary<string, int> _ronNameAssociations = new Dictionary<string, int>
    {
        {"rax", 0}, // 0000 0000 | 0x00 
        {"rbx", 1}, // 0000 0001 | 0x01
        {"pcx", 2}, // 0000 0010 | 0x02
        {"rdx", 3}, // 0000 0011 | 0x03
        {"trx", 4}, // 0000 0100 | 0x04
        {"flx", 5}, // 0000 0101 | 0x05
        {"fmx", 6}, // 0000 0110 | 0x06
    };

    public Cpu()
    {
        InitializeMemories();
        // PrintMemory(_instractionsMemory);
        // PrintMemory(_dataMemory);
        PrintMemories((new List<IList<Register>>{ _instractionsMemory, _dataMemory }));
    }

    public void PrintMemory(IList<Register> memory)
    {
        Console.ForegroundColor = (ConsoleColor)_rnd.Next(0, 15);
        foreach (var reg in memory)
        {
            Console.WriteLine(reg.ToString());
        }
        Console.ResetColor();
    }

    public void PrintMemories(IList<IList<Register>> memList)
    {
        int[] consoleColors = new int [memList.Count];
        for (int i = 0; i < consoleColors.Length; i++)
        {
            consoleColors[i] = _rnd.Next(0, 15);
        }
        for (int i = 0; i < memList[0].Count; i++)
        {
            for (int j = 0; j < memList.Count; j++)
            {
                Console.ForegroundColor = (ConsoleColor)consoleColors[j];
                Console.Write($"{memList[j][i].ToString()}\t");
            }

            Console.WriteLine();
        }
        Console.ResetColor();
    }
    
    

    private void InitializeMemories()
    {
        // Initialize Memories with registers
        for (int i = 0; i < InstructionMemorySize; i++)
        {
            _instractionsMemory.Add( new Register(RegisterSize));
            _instractionsMemory[i].SetRegisterValue(16);
            _dataMemory.Add(new Register(RegisterSize));
        }
    }

    public void SetRegisterValue(string registerName, BitArray value)
    {
        if (value.Count > RegisterSize)
            throw new ArgumentException($"Too many bits({value.Count}) in argument");
    }
}