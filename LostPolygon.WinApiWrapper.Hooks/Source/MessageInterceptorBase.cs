using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using LostPolygon.WinApiWrapper;

namespace LostPolygon.NanoWinForms {
    public abstract class MessageInterceptorBase<TMessage> {
        public delegate void ProcessMessageDelegate(ref TMessage message);

        private readonly ProcessMessageDelegate _processMessage;
        private readonly IntPtr _ownerWindowHwnd = IntPtr.Zero;
        private readonly WinApi.User32.HookProc _hookProc;
        private readonly IntPtr _moduleHandle;
        private readonly uint _threadId;
        private IntPtr _hookId = IntPtr.Zero;

        protected MessageInterceptorBase(IntPtr ownerWindowHwnd, ProcessMessageDelegate processMessage, IntPtr moduleHandle, uint threadId) {
            if (processMessage == null)
                throw new NullReferenceException("processMessage");

            _processMessage = processMessage;
            _ownerWindowHwnd = ownerWindowHwnd;
            _moduleHandle = moduleHandle;
            _threadId = threadId;

            _hookProc = HookProcInternal;
        }

        protected MessageInterceptorBase(IntPtr ownerWindowHwnd, ProcessMessageDelegate processMessage)
            : this(ownerWindowHwnd, processMessage, IntPtr.Zero, WinApi.Kernel32.GetCurrentThreadId()) {
        }

        ~MessageInterceptorBase() {
            Stop();
        }

        public IntPtr HookId {
            get {
                return _hookId;
            }
        }

        protected IntPtr OwnerWindowHwnd {
            get {
                return _ownerWindowHwnd;
            }
        }

        protected ProcessMessageDelegate ProcessMessage {
            get {
                return _processMessage;
            }
        }

        public void Start() {
            // notice that win32 callback function must be a global variable within class to avoid disposing it!
            //_hookId = WinApi.User32.SetWindowsHookEx(HookType, _hookProc, IntPtr.Zero, WinApi.Kernel32.GetCurrentThreadId());
            //IntPtr hMod = WinApi.Kernel32.LoadLibrary("user32.dll");

            _hookId = WinApi.User32.SetWindowsHookEx(HookType, _hookProc, _moduleHandle, _threadId);
            if (_hookId == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// Stop intercepting. Should be called to calm unmanaged code correctly
        /// </summary>
        public void Stop() {
            if (_hookId != IntPtr.Zero)
                WinApi.User32.UnhookWindowsHookEx(_hookId);

            _hookId = IntPtr.Zero;
        }

        protected abstract WinApi.User32.HookType HookType { get; }

        protected abstract void HookProc(IntPtr wParam, IntPtr lParam);

        private IntPtr HookProcInternal(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode < 0)
                return WinApi.User32.CallNextHookEx(_hookId, nCode, wParam, lParam);

            HookProc(wParam, lParam);

            return WinApi.User32.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
    }
}