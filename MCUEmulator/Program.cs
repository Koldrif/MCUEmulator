// See https://aka.ms/new-console-template for more information

using System.Collections;
using MCUEmulator;
using MCUEmulator.Controller;
using MCUEmulator.CPU;
using MCUEmulator.Utility;

// var cpu = CPU.Cpu;
//
// cpu.ToString();
//
// var testReg = new Register();
//
// testReg.SetValue(0xf1);
//
// Console.WriteLine(testReg.ToString());
//
// testReg.Shift(Register.ShiftDirection.Left);
//
// Console.WriteLine(testReg.ToString());
//
// testReg.Shift(Register.ShiftDirection.Right);
//
// Console.WriteLine(testReg.ToString());

CPU.Cpu.ToConsole();
AddCommand com = new AddCommand(1);

CPU.Cpu.GetRegisterByAddress(CPU.Cpu.DataMemory, 1).SetValue(1);
Console.WriteLine();
CPU.Cpu.ToConsole();

com.Execute();
Console.WriteLine();
CPU.Cpu.ToConsole();


