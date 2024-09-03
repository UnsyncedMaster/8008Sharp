using System;
using System.IO;

namespace _8008Sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            CPU cpu = new CPU();

            Console.WriteLine("8008Sharp");
            Console.WriteLine("Loading A Hardcoded Prog As A Proof Of Concept...");

            byte[] program = new byte[]
            {
                0x3E, 0x05,  // MVI A, 05h (Load Immediate Value 0x05 Into Reg A)
                0xC6, 0x03,  // ADI 03h (Add Immediate Value 0x03 To Reg A)
                0x32, 0x00, 0x10,  // STA 1000h (Store Value Of A Into Mem At Address 1000h)
                0x76         // HLT (Halt The CPU)
            };

            // Load The Prog Into Mem
            Array.Copy(program, 0, cpu.Memory, 0, program.Length);
            cpu.Reset();

            Console.WriteLine("Prog Loaded Successfully.");
            Console.WriteLine("Press 'S' To Step Through Instructions, 'R' To Run, OR 'Q' To Quit.");

            bool running = true;
            while (running)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (char.ToUpper(key))
                {
                    case 'S':
                        Step(cpu);
                    break;
                    case 'R':
                        Run(cpu);
                    break;
                    case 'Q':
                        running = false;
                    break;
                    default:
                        Console.WriteLine("Invalid Cmd. Use 'S' To step, 'R' To Run, OR 'Q' To Quit.");
                    break;
                }
            }

            Console.WriteLine("Emulator Termed.");
        }

        static void Step(CPU cpu)
        {
            cpu.ExecuteInstruction(cpu.Memory[cpu.PC++]);
            PrintState(cpu);
        }

        static void Run(CPU cpu)
        {
            Console.WriteLine("Running Prog... Press 'Q' To Stop.");

            while (cpu.PC < cpu.Memory.Length)
            {
                cpu.ExecuteInstruction(cpu.Memory[cpu.PC++]);
                PrintState(cpu);

                if (Console.KeyAvailable && Console.ReadKey(true).KeyChar == 'Q')
                {
                    break;
                }
            }

            Console.WriteLine("Program Exec Finished!");
        }

        static void PrintState(CPU cpu)
        {
            Console.Clear();
            Console.WriteLine("CPU State:");
            Console.WriteLine($"A:  {cpu.A:X2}");
            Console.WriteLine($"B:  {cpu.B:X2}");
            Console.WriteLine($"C:  {cpu.C:X2}");
            Console.WriteLine($"D:  {cpu.D:X2}");
            Console.WriteLine($"E:  {cpu.E:X2}");
            Console.WriteLine($"H:  {cpu.H:X2}");
            Console.WriteLine($"L:  {cpu.L:X2}");
            Console.WriteLine($"PC: {cpu.PC:X4}");
            Console.WriteLine($"SP: {cpu.SP:X4}");
            Console.WriteLine("Flags:");
            Console.WriteLine($"Zero: {cpu.ZeroFlag}");
            Console.WriteLine($"Sign: {cpu.SignFlag}");
            Console.WriteLine($"Parity: {cpu.ParityFlag}");
            Console.WriteLine($"Carry: {cpu.CarryFlag}");
        }
    }
}

