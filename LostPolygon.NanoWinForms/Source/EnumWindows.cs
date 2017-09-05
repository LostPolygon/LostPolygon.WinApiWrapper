//********************************************************************************************
//Author: Sergey Stoyan, CliverSoft Co.
//        stoyan@cliversoft.com
//        sergey.stoyan@gmail.com
//        http://www.cliversoft.com
//        07 September 2006
//Copyright: (C) 2006, Sergey Stoyan
//
//Modified: Serhii Yolkin
//        http://LostPolygon.com
//        contact@lostpolygon.com
//********************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using LostPolygon.WinApiWrapper;

namespace LostPolygon.NanoWinForms {
    public class EnumWindows {
        private readonly uint _processId;
        private readonly bool _includeChildWindows;
        private readonly WinApi.User32.EnumProc _enumWindowCallback;
        private readonly WinApi.User32.EnumProc _enumChildWindowCallback;
        private readonly List<Window> _windows = new List<Window>();

        public ICollection<Window> Windows {
            get {
                return _windows;
            }
        }

        public EnumWindows(bool includeChildWindows, uint processId) {
            _includeChildWindows = includeChildWindows;
            _processId = processId;
            _enumWindowCallback = EnumWindowCallback;
            _enumChildWindowCallback = EnumChildWindowCallback;

            _windows.Clear();
        }

        public void Enumerate() {
            _windows.Clear();
            WinApi.User32.EnumWindows(_enumWindowCallback, 0);
        }

        private bool EnumWindowCallback(IntPtr hWnd, int lValue) {
            uint windowProcessId;
            WinApi.User32.GetWindowThreadProcessId(hWnd, out windowProcessId);

            if (_processId != windowProcessId)
                return true;

            Window w = new Window();

            w.Level = 0;
            w.ParentId = -1;
            w.HWnd = hWnd;

            StringBuilder sb = new StringBuilder(255);

            WinApi.User32.GetClassName(hWnd, sb, 255);
            w.ClassName = sb.ToString();
            WinApi.User32.GetWindowText(hWnd, sb, 255);
            w.Text = sb.ToString();
            w.Path = "[" + w.Text + "]";

            _windows.Add(w);

            if (_includeChildWindows) {
                WinApi.User32.EnumChildWindows(hWnd, _enumChildWindowCallback, _windows.Count - 1);
            }

            return true;
        }

        private bool EnumChildWindowCallback(IntPtr hWnd, int parentId) {
            uint windowProcessId;
            WinApi.User32.GetWindowThreadProcessId(hWnd, out windowProcessId);

            if (_processId != windowProcessId)
                return true;

            Window w = new Window();

            w.Level = _windows[parentId].Level + 1;
            w.ParentId = parentId;
            w.HWnd = hWnd;

            StringBuilder s = new StringBuilder(255);

            WinApi.User32.GetClassName(hWnd, s, 255);
            w.ClassName = s.ToString();
            WinApi.User32.GetWindowText(hWnd, s, 255);
            w.Text = s.ToString();
            w.Path = _windows[parentId].Path + "[" + w.Text + "]";

            WinApi.User32.GetWindowRect(w.HWnd, out w.Rect);

            _windows.Add(w);
            WinApi.User32.EnumChildWindows(hWnd, _enumChildWindowCallback, _windows.Count - 1);

            return true;
        }

        public class Window {
            public IntPtr HWnd;
            public string Path;
            public string Text;
            public string ClassName;
            public int Level;
            public int ParentId;
            public WinApi.RECT Rect;
        }
    }
}