using System;
using System.Runtime.InteropServices;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        [StructLayout(LayoutKind.Sequential)]
        public struct MSG {
            public IntPtr hwnd;
            public MessageType message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }
    }
}
