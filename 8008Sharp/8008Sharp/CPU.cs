using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8008Sharp
{
    public class CPU
    {
        public byte A, B, C, D, E, H, L;
        public ushort SP;
        public ushort PC;
        public bool ZeroFlag, SignFlag, ParityFlag, CarryFlag;
        public byte[] Memory; // 16 KB Mem
        public bool Halted { get; private set; } = false;

        public CPU()
        {
            Memory = new byte[16 * 1024]; // 16 KB Mem
            Reset();
        }

        public void Reset()
        {
            A = B = C = D = E = H = L = 0;
            SP = 0;
            PC = 0;
            ZeroFlag = SignFlag = ParityFlag = CarryFlag = false;
        }

        public void Run()
        {
            while (true)
            {
                byte opcode = Memory[PC++]; // Fetch
                ExecuteInstruction(opcode); // Decode and Execute
            }
        }

        public void ExecuteInstruction(byte opcode)
        {
            switch (opcode)
            {
                case 0x00:
                break;
                case 0x3E: 
                    A = Memory[PC++];
                break;
                case 0x80: 
                    Add(B);
                break;
                case 0xC6: 
                    byte valueToAdd = Memory[PC++];
                    int result = A + valueToAdd;

                    ZeroFlag = (result & 0xFF) == 0;
                    SignFlag = (result & 0x80) != 0;
                    CarryFlag = result > 0xFF;
                    ParityFlag = CalculateParity(result & 0xFF);

                    A = (byte)(result & 0xFF);
                break;
                case 0x32:
                    ushort addr = (ushort)(Memory[PC++] | (Memory[PC++] << 8));
                    Memory[addr] = A;
                break;
                case 0x76:
                    Halted = true;
                break;
                case 0x08:
                    Console.WriteLine("nothing special just a not existing opcode that keeps popping up for no reason cuz UwU");
                break;
                default:
                    //Console.WriteLine($"Unknown Opcode {opcode:X2} At Address {PC - 1:X4}");
                    throw new InvalidOperationException($"Unknown Opcode {opcode:X2}");
                    //Console.WriteLine($"Ignoring Unknown Opcode {opcode:X2} At Address {PC - 1:X4} Cuz I'm A Itty Bitty Wetard UwU");
                    PC++;
                    Halted = true;
                break;
            }
        }
        private bool CalculateParity(int value)
        {
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((value & (1 << i)) != 0)
                {
                    count++;
                }
            }
            return (count % 2) == 0;
        }
        private void Add(byte value)
        {
            int result = A + value;
            CarryFlag = result > 0xFF;
            A = (byte)result;
            ZeroFlag = A == 0;
            SignFlag = (A & 0x80) != 0;
            ParityFlag = (A & 0x01) == 0;
        }

        public byte ReadMemory(ushort address)
        {
            return Memory[address];
        }

        public void WriteMemory(ushort address, byte value)
        {
            Memory[address] = value;
        }
    }
}
