using System;
using System.Runtime.InteropServices;
using LostPolygon.WinApiWrapper;

namespace LostPolygon.NanoWinForms {
    public class GetMessageInterceptor : MessageInterceptorBase<WinApi.MSG> {
        public GetMessageInterceptor(IntPtr ownerWindowHwnd, ProcessMessageDelegate processMessage, IntPtr moduleHandle, uint threadId)
             : base(ownerWindowHwnd, processMessage, moduleHandle, threadId) {
        }

        public GetMessageInterceptor(IntPtr ownerWindowHwnd, ProcessMessageDelegate processMessage) : base(ownerWindowHwnd, processMessage) {
        }

        protected override WinApi.User32.HookType HookType {
            get {
                return WinApi.User32.HookType.WH_GETMESSAGE;
            }
        }

        protected override void HookProc(IntPtr wParam, IntPtr lParam) {
            WinApi.MSG msg = (WinApi.MSG) Marshal.PtrToStructure(lParam, typeof(WinApi.MSG));
            if (msg.hwnd == OwnerWindowHwnd && ProcessMessage != null) {
                ProcessMessage(ref msg);
                Marshal.StructureToPtr(msg, lParam, false);
            }
        }
    }

}