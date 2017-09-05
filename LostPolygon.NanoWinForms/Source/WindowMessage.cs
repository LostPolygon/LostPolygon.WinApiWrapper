using System;
using LostPolygon.WinApiWrapper;

namespace LostPolygon.NanoWinForms {
    public struct WindowMessage {
        public readonly IntPtr HWnd;
        public readonly WinApi.MessageType MessageType;
        public readonly IntPtr WParam;
        public readonly IntPtr LParam;
        public IntPtr Result;

        public WindowMessage(IntPtr hWnd, WinApi.MessageType messageType, IntPtr wParam, IntPtr lParam) : this() {
            HWnd = hWnd;
            MessageType = messageType;
            WParam = wParam;
            LParam = lParam;
        }
    }
}
