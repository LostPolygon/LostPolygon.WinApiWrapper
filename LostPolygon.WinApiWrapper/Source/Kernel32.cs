using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        public static class Kernel32 {
            #region Constants

            #endregion Constants

            #region Delegates

            #endregion Delegates

            #region Functions

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int GetLastError();

            [DllImport("kernel32.dll")]
            public static extern uint GetCurrentThreadId();

            [DllImport("kernel32.dll")]
            public static extern uint GetCurrentProcess();

            [DllImport("kernel32.dll")]
            public static extern uint GetCurrentProcessId();

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetModuleHandle(string module);

            [DllImport("kernel32", SetLastError=true)]
            public static extern IntPtr LoadLibrary(string fileName);

            [DllImport("kernel32.dll",SetLastError=true)]
            public static extern int SuspendThread(IntPtr hThread);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint ResumeThread(IntPtr hThread);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

            [DllImport("kernel32.dll", SetLastError=true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseHandle(IntPtr hObject);

            #endregion Functions

            #region Enums

            [Flags]
            public enum ThreadAccess {
                TERMINATE = 0x0001,
                SUSPEND_RESUME = 0x0002,
                GET_CONTEXT = 0x0008,
                SET_CONTEXT = 0x0010,
                SET_INFORMATION = 0x0020,
                QUERY_INFORMATION = 0x0040,
                SET_THREAD_TOKEN = 0x0080,
                IMPERSONATE = 0x0100,
                DIRECT_IMPERSONATION = 0x0200
            }

            #endregion
        }
    }
}
