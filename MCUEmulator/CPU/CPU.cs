using System.Collections;
using MCUEmulator.CPU;

namespace MCUEmulator;

//TODO Make it singleton!
public class Cpu
{
    /// <summary>
    /// Describes how many registers will Instruction and Data memory will contain
    /// </summary>
    private const int InstructionMemorySize = 34;
    
    
    /// <summary>
    /// Describes how many triggers will be in registers 
    /// </summary>
    private const int RegisterSize = 8;

    private Random _rnd = new Random((int)DateTime.Now.Ticks);
    ///<summary>
    /// Instruction Memory
    /// </summary>
    private static List<Register> _instractionsMemory = new(InstructionMemorySize);
    /// <summary>
    /// Data Memory
    /// </summary>
    private static List<Register> _dataMemory = new(InstructionMemorySize);

    /// <summary>
    /// Map that connects registers names with their <inheritdoc cref="_dataMemory"/> and address in it  
    /// </summary>
    private Dictionary<string, (List<Register>, int)> _ronNameAssociations = new Dictionary<string, (List<Register>, int)>
    {
        {"rax", (_dataMemory, 0)}, // 0000 0000 | 0x00 
        {"rbx", (_dataMemory, 1)}, // 0000 0001 | 0x01
        {"pcx", (_dataMemory, 2)}, // 0000 0010 | 0x02
        {"rdx", (_dataMemory, 3)}, // 0000 0011 | 0x03
        {"trx", (_dataMemory, 4)}, // 0000 0100 | 0x04
        {"flx", (_dataMemory, 5)}, // 0000 0101 | 0x05
        {"fmx", (_dataMemory, 6)}, // 0000 0110 | 0x06
    };
    
    public Cpu()
    {
        InitializeMemories();
        // PrintMemory(_instructionsMemory);
        // PrintMemory(_dataMemory);
        PrintMemories((new List<IList<Register>>{ _instractionsMemory, _dataMemory }));
    }
    /// <summary>
    /// Prints register of given registers 
    /// </summary>
    /// <param name="memory">List of registers to print</param>
    public void PrintMemory(IList<Register> memory)
    {
        Console.ForegroundColor = (ConsoleColor)_rnd.Next(0, 15);
        foreach (var reg in memory)
        {
            Console.WriteLine(reg.ToString());
        }
        Console.ResetColor();
    }
    /// <summary>
    /// Prints register of given registers 
    /// </summary>
    /// <param name="memList">List of memories to print</param>
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
    
    
    /// <summary>
    /// Load registers into memory
    /// </summary>
    private void InitializeMemories()
    {
        // Initialize Memories with registers
        for (int i = 0; i < InstructionMemorySize; i++)
        {
            _instractionsMemory.Add( new Register(RegisterSize));
            _instractionsMemory[i].SetRegisterValue(0b1_0000_0000);
            _dataMemory.Add(new Register(RegisterSize));
        }
    }
    /// <summary>
    /// Set value of register with given name
    /// </summary>
    /// <param name="registerName">Name of register to load data in</param>
    /// <param name="value"><see cref="BitArray"/> that will be load into register</param>
    /// <exception cref="ArgumentException">If value bigger, than register can hold</exception>
    public void SetRegisterValue(string registerName, BitArray value)
    {
        if (value.Count > RegisterSize)
            throw new ArgumentException($"Too many bits({value.Count}) in argument");
        else
        {
            var register = GetRegisterByName(registerName);
            register.SetRegisterValue(value);
        }
    }
    
    /// <summary>
    /// Returns register by name
    /// </summary>
    /// <param name="registerName">name of register</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">if name of register doesn't exist</exception>
    private Register GetRegisterByName(string registerName)
    {
        if (_ronNameAssociations.ContainsKey(registerName))
        {
            var registerAddress = _ronNameAssociations[registerName];
            var memory = registerAddress.Item1;
            return memory[registerAddress.Item2];
        }
        else
        {
            throw new ArgumentException("Unknown register name");
        }
    }
    
    /// <summary>
    /// Return register with given address from given memory 
    /// </summary>
    /// <param name="memory">where to search register</param>
    /// <param name="address">Register address</param>
    /// <returns></returns>
    private Register GetRegisterByAddress(List<Register> memory, int address)
    {
        return memory[address];
    }
    
    
}