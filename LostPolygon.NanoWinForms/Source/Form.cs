using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using LostPolygon.WinApiWrapper;

namespace LostPolygon.NanoWinForms {
    public class Form : IDisposable {
        private readonly IntPtr _moduleHandle;
        private readonly ApplicationContext _applicationContext;
        private IntPtr _hWnd;
        private WinApi.User32.WindowClassStyles _classStyle = WinApi.User32.WindowClassStyles.CS_HREDRAW | WinApi.User32.WindowClassStyles.CS_VREDRAW;
        private WinApi.User32.WindowStyles _style = WinApi.User32.WindowStyles.WS_OVERLAPPEDWINDOW;
        private WinApi.User32.WindowStylesEx _styleEx = WinApi.User32.WindowStylesEx.WS_EX_APPWINDOW;
        private IntPtr _backgroundBrush;
        private IntPtr _cursor;
        private IntPtr _smallIcon;
        private IntPtr _icon;
        private string _text;

        private WinApi.RECT _rect =
            new WinApi.RECT(
                (int) WinApi.User32.CreateWindowParameters.CW_USEDEFAULT,
                (int) WinApi.User32.CreateWindowParameters.CW_USEDEFAULT,
                (int) WinApi.User32.CreateWindowParameters.CW_USEDEFAULT,
                (int) WinApi.User32.CreateWindowParameters.CW_USEDEFAULT
                );

        public WinApi.User32.WindowClassStyles ClassStyle {
            get {
                if (!IsHandleCreated)
                    return _classStyle;

                return (WinApi.User32.WindowClassStyles) WinApi.User32.GetClassLongInteger(Handle, WinApi.User32.GetClassLongParameter.GCL_STYLE);
            }
            set {
                if (!IsHandleCreated) {
                    _classStyle = value;
                    return;
                }

                WinApi.User32.SetClassLong(Handle, WinApi.User32.GetClassLongParameter.GCL_STYLE, (long) value);
            }
        }

        public WinApi.User32.WindowStyles Style {
            get {
                if (!IsHandleCreated)
                    return _style;

                return (WinApi.User32.WindowStyles) WinApi.User32.GetWindowLongInteger(Handle, WinApi.User32.GetWindowLongParameter.GWL_STYLE);
            }
            set {
                if (!IsHandleCreated) {
                    _style = value;
                    return;
                }

                WinApi.User32.SetWindowLong(Handle, WinApi.User32.GetWindowLongParameter.GWL_STYLE, (long) value);
            }
        }

        public WinApi.User32.WindowStylesEx StyleEx {
            get {
                if (!IsHandleCreated)
                    return _styleEx;

                return (WinApi.User32.WindowStylesEx) WinApi.User32.GetWindowLongInteger(Handle, WinApi.User32.GetWindowLongParameter.GWL_EXSTYLE);
            }
            set {
                if (!IsHandleCreated) {
                    _styleEx = value;
                    return;
                }

                WinApi.User32.SetWindowLong(Handle, WinApi.User32.GetWindowLongParameter.GWL_EXSTYLE, (long) value);
            }
        }

        public string Text {
            get {
                if (!IsHandleCreated)
                    return _text ?? "";

                // it's okay to call GetWindowText cross-thread.
                int textLen = WinApi.User32.GetWindowTextLength(Handle);
                StringBuilder sb = new StringBuilder(textLen + 1);
                WinApi.User32.GetWindowText(Handle, sb, sb.Capacity);
                return sb.ToString();
            }
            set {
                if (value == null)
                    value = "";

                if (Text.Equals(value))
                    return;

                if (!IsHandleCreated) {
                    _text = value.Length == 0 ? null : value;
                    return;
                }

                WinApi.User32.SetWindowText(Handle, value);
            }
        }

        public IntPtr Icon {
            get {
                if (!IsHandleCreated)
                    return _icon;

                return GetIcon(Handle);
            }
            set {
                if (!IsHandleCreated) {
                    _icon = value;
                    return;
                }

                SendMessage(WinApi.MessageType.WM_SETICON, (int) WinApi.User32.WindowIconType.ICON_BIG, value);
                RedrawFrame();
            }
        }

        public IntPtr SmallIcon {
            get {
                if (!IsHandleCreated)
                    return _smallIcon;

                return GetSmallIcon(Handle);
            }
            set {
                if (!IsHandleCreated) {
                    _smallIcon = value;
                    return;
                }

                SendMessage(WinApi.MessageType.WM_SETICON, (int) WinApi.User32.WindowIconType.ICON_SMALL, value);
                RedrawFrame();
            }
        }

        public IntPtr Cursor {
            get {
                if (!IsHandleCreated)
                    return _cursor;

                return WinApi.User32.GetClassLong(Handle, WinApi.User32.GetClassLongParameter.GCL_HCURSOR);
            }
            set {
                if (!IsHandleCreated) {
                    _cursor = value;
                    return;
                }

                WinApi.User32.SetClassLong(Handle, WinApi.User32.GetClassLongParameter.GCL_HCURSOR, value);
            }
        }

        public IntPtr BackgroundBrush {
            get {
                if (!IsHandleCreated)
                    return _backgroundBrush;

                return WinApi.User32.GetClassLong(Handle, WinApi.User32.GetClassLongParameter.GCL_HBRBACKGROUND);
            }
            set {
                if (!IsHandleCreated) {
                    _backgroundBrush = value;
                    return;
                }

                WinApi.User32.SetClassLong(Handle, WinApi.User32.GetClassLongParameter.GCL_HBRBACKGROUND, value);
            }
        }

        public WinApi.RECT Rect {
            get {
                WinApi.RECT rect;
                WinApi.User32.GetWindowRect(Handle, out rect);
                return rect;
            }
            set { UpdateRect(value); }
        }

        public WinApi.RECT ClientRect {
            get {
                WinApi.RECT rect;
                WinApi.User32.GetClientRect(Handle, out rect);
                return rect;
            }
        }

        private void UpdateRect(WinApi.RECT newRect) {
            if (!IsHandleCreated) {
                _rect = newRect;
                return;
            }

            WinApi.RECT oldRect = _rect;
            _rect = newRect;
            if (oldRect == newRect)
                return;

            WinApi.User32.SetWindowPosFlags flags = WinApi.User32.SetWindowPosFlags.SWP_NOZORDER | WinApi.User32.SetWindowPosFlags.SWP_NOACTIVATE;
            if (_rect.X == oldRect.X && _rect.Y == oldRect.Y) {
                flags |= WinApi.User32.SetWindowPosFlags.SWP_NOMOVE;
            }
            if (_rect.Width == oldRect.Width && _rect.Height == oldRect.Height) {
                flags |= WinApi.User32.SetWindowPosFlags.SWP_NOSIZE;
            }

            WinApi.User32.SetWindowPos(Handle, IntPtr.Zero, _rect.X, _rect.Y, _rect.Width, _rect.Height, flags);
        }

        public IntPtr Handle {
            get { return _hWnd; }
        }

        protected bool IsHandleCreated {
            get { return _hWnd != IntPtr.Zero; }
        }

        public Form(ApplicationContext applicationContext) {
            _applicationContext = applicationContext;
            _moduleHandle = WinApi.Kernel32.GetModuleHandle(null);
            Icon = WinApi.User32.LoadIcon(WinApi.User32.IconId.IDI_APPLICATION);
            SmallIcon = Icon;
            Cursor = WinApi.User32.LoadCursor(WinApi.User32.CursorId.IDC_ARROW);
            BackgroundBrush = new IntPtr((int) WinApi.User32.SystemColorId.COLOR_WINDOW);
        }

        public Form(IntPtr hWnd) {
            _moduleHandle = WinApi.Kernel32.GetModuleHandle(null);
            _hWnd = hWnd;
        }

        public void Show() {
            if (!IsHandleCreated) {
                CreateHandle();
            }

            WinApi.User32.ShowWindow(Handle, WinApi.User32.ShowWindowCommands.SW_SHOW);
        }

        public void Hide() {
            if (!IsHandleCreated)
                return;

            WinApi.User32.ShowWindow(Handle, WinApi.User32.ShowWindowCommands.SW_HIDE);
        }

        public void Close() {
            if (IsHandleCreated) {
                SendMessage(WinApi.MessageType.WM_CLOSE, 0, 0);
            } else {
                Dispose();
            }
        }

        protected virtual void CreateHandle() {
            WinApi.User32.WNDCLASSEX windowClassEx = GetWindowClassEx();
            if (WinApi.User32.RegisterClassEx(ref windowClassEx) == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            _hWnd = WinApi.User32.CreateWindowEx(
                StyleEx,
                windowClassEx.lpszClassName,
                Text,
                Style,
                _rect.X, _rect.Y,
                _rect.Width, _rect.Height,
                IntPtr.Zero,
                IntPtr.Zero,
                windowClassEx.hInstance,
                IntPtr.Zero);

            if (_hWnd == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }


        protected virtual WinApi.User32.WNDCLASSEX GetWindowClassEx() {
            WinApi.User32.WNDCLASSEX wndClassEx = new WinApi.User32.WNDCLASSEX();
            wndClassEx.cbSize = WinApi.User32.WNDCLASSEX.SizeOf;
            wndClassEx.style = ClassStyle;
            wndClassEx.lpfnWndProc = WndProcInternal;
            wndClassEx.cbClsExtra = 0;
            wndClassEx.cbWndExtra = 0;
            wndClassEx.hInstance = _moduleHandle;
            wndClassEx.hIcon = Icon;
            wndClassEx.hIconSm = SmallIcon;
            wndClassEx.hCursor = Cursor;
            wndClassEx.hbrBackground = BackgroundBrush;
            wndClassEx.lpszClassName = "LPWindowClass_" + GetHashCode();

            return wndClassEx;
        }

        private IntPtr WndProcInternal(IntPtr hWnd, WinApi.MessageType msg, IntPtr wParam, IntPtr lParam) {
            WindowMessage message = new WindowMessage(hWnd, msg, wParam, lParam);
            return WndProc(ref message);
        }

        protected virtual IntPtr WndProc(ref WindowMessage message) {
            switch (message.MessageType) {
                case WinApi.MessageType.WM_CREATE:
                    WmCreate(ref message);
                    break;
                case WinApi.MessageType.WM_DESTROY:
                    WmDestroy(ref message);
                    break;
                default:
                    DefWndProc(ref message);
                    break;
            }

            return message.Result;
        }

        private void WmCreate(ref WindowMessage message) {
            DefWndProc(ref message);
            if (_applicationContext != null) {
                _applicationContext.AddForm(this);
            }

            Dispose();
        }

        private void WmDestroy(ref WindowMessage message) {
            DefWndProc(ref message);
            if (_applicationContext != null) {
                _applicationContext.RemoveForm(this);
            }
        }

        private void DefWndProc(ref WindowMessage message) {
            message.Result = WinApi.User32.DefWindowProc(message.HWnd, message.MessageType, message.WParam, message.LParam);
        }

        private void InvalidateStyles() {
            WinApi.User32.SetWindowPos(
                Handle, IntPtr.Zero, 0, 0, 0, 0,
                WinApi.User32.SetWindowPosFlags.SWP_DRAWFRAME |
                WinApi.User32.SetWindowPosFlags.SWP_NOACTIVATE |
                WinApi.User32.SetWindowPosFlags.SWP_NOMOVE |
                WinApi.User32.SetWindowPosFlags.SWP_NOSIZE |
                WinApi.User32.SetWindowPosFlags.SWP_NOZORDER
                );
        }

        public void HandleStyleChange() {
            WinApi.User32.SetWindowPos(
                Handle, IntPtr.Zero, 0, 0, 0, 0,
                WinApi.User32.SetWindowPosFlags.SWP_FRAMECHANGED |
                WinApi.User32.SetWindowPosFlags.SWP_NOACTIVATE |
                WinApi.User32.SetWindowPosFlags.SWP_NOMOVE |
                WinApi.User32.SetWindowPosFlags.SWP_NOSIZE |
                WinApi.User32.SetWindowPosFlags.SWP_NOZORDER
                );
        }

        private void RedrawFrame() {
            WinApi.User32.RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, WinApi.User32.RedrawWindowFlags.RDW_INVALIDATE | WinApi.User32.RedrawWindowFlags.RDW_FRAME);
        }

        private IntPtr SendMessage(WinApi.MessageType msg, IntPtr wparam, IntPtr lparam) {
            return WinApi.User32.SendMessage(Handle, msg, wparam, lparam);
        }

        private IntPtr SendMessage(WinApi.MessageType msg, IntPtr wparam, int lparam) {
            return WinApi.User32.SendMessage(Handle, msg, wparam, (IntPtr) lparam);
        }

        private IntPtr SendMessage(WinApi.MessageType msg, int wparam, IntPtr lparam) {
            return WinApi.User32.SendMessage(Handle, msg, (IntPtr) wparam, lparam);
        }

        private IntPtr SendMessage(WinApi.MessageType msg, int wparam, int lparam) {
            return WinApi.User32.SendMessage(Handle, msg, (IntPtr) wparam, (IntPtr) lparam);
        }

        private static IntPtr GetIcon(IntPtr hWnd) {
            IntPtr icon = WinApi.User32.SendMessage(hWnd, WinApi.MessageType.WM_GETICON, (IntPtr) WinApi.User32.WindowIconType.ICON_BIG, IntPtr.Zero);
            if (icon == IntPtr.Zero)
                icon = WinApi.User32.GetClassLong(hWnd, WinApi.User32.GetClassLongParameter.GCL_HICON);

            return icon;
        }

        private static IntPtr GetSmallIcon(IntPtr hWnd) {
            IntPtr icon = WinApi.User32.SendMessage(hWnd, WinApi.MessageType.WM_GETICON, (IntPtr) WinApi.User32.WindowIconType.ICON_SMALL2, IntPtr.Zero);
            if (icon == IntPtr.Zero)
                icon = WinApi.User32.SendMessage(hWnd, WinApi.MessageType.WM_GETICON, (IntPtr) WinApi.User32.WindowIconType.ICON_SMALL, IntPtr.Zero);
            if (icon == IntPtr.Zero)
                icon = WinApi.User32.GetClassLong(hWnd, WinApi.User32.GetClassLongParameter.GCL_HICONSM);

            return icon;
        }

        public virtual void Dispose() {
        }
    }
}
