using System;
using LostPolygon.WinApiWrapper;

namespace LostPolygon.NanoWinForms {
    public class MessagePump {
        private readonly IntPtr _hWnd;
        private bool _isRunning;

        public bool IsRunning {
            get { return _isRunning; }
        }

        public MessagePump() {
        }

        public MessagePump(IntPtr hWnd) {
            _hWnd = hWnd;
        }

        public virtual void Run() {
            if (IsRunning)
                throw new InvalidOperationException("Already running");

            _isRunning = true;
            WinApi.MSG message;
            while (WinApi.User32.GetMessage(out message, _hWnd, 0, 0) > 0) {
                WinApi.User32.TranslateMessage(ref message);
                WinApi.User32.DispatchMessage(ref message);
            }
        }

        public virtual void Quit() {
            if (!IsRunning)
                throw new InvalidOperationException("Not running");

            _isRunning = false;
            WinApi.User32.PostQuitMessage(0);
        }
    }
}