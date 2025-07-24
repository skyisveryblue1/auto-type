using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace auto_type
{
    public class GlobalMouseHook : IDisposable
    {
        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;


        private LowLevelMouseProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private bool _disposed = false;

        public event EventHandler<MouseEventArgs> MouseClick;

        public GlobalMouseHook()
        {
            Console.WriteLine("Initializing GlobalMouseHook...");
            _proc = HookCallback;
            _hookID = SetHook(_proc);
            Console.WriteLine($"Hook set: {_hookID != IntPtr.Zero}");
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                IntPtr hook = SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                if (hook == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    Console.WriteLine($"Failed to set hook. Error code: {errorCode}");
                    throw new System.ComponentModel.Win32Exception(errorCode, "Failed to set mouse hook");
                }
                return hook;
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_LBUTTONUP))
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseButtons button = MouseButtons.Left;

                MouseClick?.Invoke(this, new MouseEventArgs(
                    button,
                    1,
                    hookStruct.pt.x,
                    hookStruct.pt.y,
                    0));
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn,
            IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public void Dispose()
        {
            if (_disposed)
            {
                Console.WriteLine("Dispose already called.");
                return;
            }

            Console.WriteLine("Calling UnhookWindowsHookEx...");
            bool result = UnhookWindowsHookEx(_hookID);
            Console.WriteLine($"UnhookWindowsHookEx result: {result}, HookID: {_hookID}");
            if (!result)
            {
                int errorCode = Marshal.GetLastWin32Error();
                Console.WriteLine($"UnhookWindowsHookEx failed. Error code: {errorCode}");
            }
            _disposed = true;
        }
    }
}
