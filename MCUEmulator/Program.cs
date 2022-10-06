// See https://aka.ms/new-console-template for more information

using MCUEmulator;
using MCUEmulator.CPU;

// var cpu = new Cpu();

var testReg = new Register();

testReg.SetRegisterValue(0xf1);

Console.WriteLine(testReg.ToString());

testReg.Shift(Register.ShiftDirection.Left);

Console.WriteLine(testReg.ToString());

testReg.Shift(Register.ShiftDirection.Right);

Console.WriteLine(testReg.ToString());

          
