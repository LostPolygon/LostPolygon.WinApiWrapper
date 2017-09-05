using System;
using System.Runtime.InteropServices;
using LostPolygon.WinApiWrapper;

namespace LostPolygon.NanoWinForms {
    public class WindowProcRetInterceptor : MessageInterceptorBase<WinApi.User32.CWPRETSTRUCT> {
        public WindowProcRetInterceptor(IntPtr ownerWindowHwnd, ProcessMessageDelegate processMessage, IntPtr moduleHandle, uint threadId)
             : base(ownerWindowHwnd, processMessage, moduleHandle, threadId) {
        }

        public WindowProcRetInterceptor(IntPtr ownerWindowHwnd, ProcessMessageDelegate processMessage) 
            : base(ownerWindowHwnd, processMessage) {
        }

        protected override WinApi.User32.HookType HookType {
            get {
                return WinApi.User32.HookType.WH_CALLWNDPROCRET;
            }
        }

        protected override void HookProc(IntPtr wParam, IntPtr lParam) {
            WinApi.User32.CWPRETSTRUCT msg = (WinApi.User32.CWPRETSTRUCT) Marshal.PtrToStructure(lParam, typeof(WinApi.User32.CWPRETSTRUCT));
            if (msg.hwnd == OwnerWindowHwnd && ProcessMessage != null) {
                ProcessMessage(ref msg);
                Marshal.StructureToPtr(msg, lParam, false);
            }
        }
    }
}