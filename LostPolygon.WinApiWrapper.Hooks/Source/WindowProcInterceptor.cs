using System;
using System.Runtime.InteropServices;
using LostPolygon.WinApiWrapper;

namespace LostPolygon.NanoWinForms {
    public class WindowProcInterceptor : MessageInterceptorBase<WinApi.User32.CWPSTRUCT> {
        public WindowProcInterceptor(IntPtr ownerWindowHwnd, ProcessMessageDelegate processMessage, IntPtr moduleHandle, uint threadId)
             : base(ownerWindowHwnd, processMessage, moduleHandle, threadId) {
        }

        public WindowProcInterceptor(IntPtr ownerWindowHwnd, ProcessMessageDelegate processMessage) : base(ownerWindowHwnd, processMessage) {
        }

        protected override WinApi.User32.HookType HookType {
            get {
                return WinApi.User32.HookType.WH_CALLWNDPROC;
            }
        }

        protected override void HookProc(IntPtr wParam, IntPtr lParam) {
            WinApi.User32.CWPSTRUCT msg = (WinApi.User32.CWPSTRUCT) Marshal.PtrToStructure(lParam, typeof(WinApi.User32.CWPSTRUCT));
            if (msg.hwnd == OwnerWindowHwnd && ProcessMessage != null) {
                ProcessMessage(ref msg);
                Marshal.StructureToPtr(msg, lParam, false);
            }
        }
    }
}