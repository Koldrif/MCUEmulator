// See https://aka.ms/new-console-template for more information

using MCUEmulator;
using MCUEmulator.CPU;
using MCUEmulator.Lexer;

// var cpu = new Cpu();

/*var testReg = new Register();

testReg.SetRegisterValue(0xf1);

Console.WriteLine(testReg.ToString());

testReg.Shift(Register.ShiftDirection.Left);

Console.WriteLine(testReg.ToString());

testReg.Shift(Register.ShiftDirection.Right);

Console.WriteLine(testReg.ToString());*/





    StreamReader stream = OpenFileVisualInterface(args);
    List<Token> tokens;
    try
    {
        // Получаем токены.
        tokens = new LexerLang().SearchTokens(stream);
    }
    catch(LexerException e)
    {
        // Ошибка.
        Console.WriteLine(e + "\n" + e.StackTrace);
        return 5;
    }
    stream.Close();
    foreach (Token token in tokens)
        // Печатаем токины.
        Console.WriteLine(token);
    Console.Write("Press eny key...");
    Console.ReadLine();


static StreamReader OpenFileVisualInterface(string[] args)
{
    FileInfo input;
    if (args.Length != 1)
    { // Если файл из аргументов программы не взят
        Console.Write("Name file: ");
        input = new FileInfo(Console.ReadLine());
    }
    else
    { // Если есть аргументы, то берём из аргументов.
        input = new FileInfo(args[0]);
    }
    return input.OpenText();
}

    return 0;

 
