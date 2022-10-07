using MCUEmulator.Utility;

namespace MCUEmulator.Controller;

public class Command
{
    public static CPU.CPU CpuInstance = CPU.CPU.Cpu;
}

public interface ICommand
{
    void Execute();
}

public class AddCommand : Command, ICommand
{

    private int _op1;
    private int _op2;
    private int _op3;
    
    public AddCommand(int op1)
    {
        _op1 = op1;
        _op2 = -1;
        _op3 = -1;
        
    }
    public AddCommand(int op1, int op2)
    {
        _op1 = op1;
        _op2 = op2;
        _op3 = -1;
    }
    public AddCommand(int op1, int op2, int op3)
    {
        _op1 = op1;
        _op2 = op2;
        _op3 = op3;

    }
    
    public void Execute()
    {
        if (_op2 < 0 && _op3 < 0)
        {
            // Take value from register by address
            var value = CpuInstance.DataMemory[_op1].Bits.ToInt();
            // Add op1 to accomulator
            var reg = CpuInstance.GetRegisterByName("rax");
            var result = reg.Bits.ToInt() + value;
            reg.SetValue(result);
        }
    }
}