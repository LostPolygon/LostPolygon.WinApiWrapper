using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        public static class User32 {
            #region Constants

            public const int GW_HWNDFIRST = 0;
            public const int GW_HWNDLAST = 1;
            public const int GW_HWNDNEXT = 2;
            public const int GW_HWNDPREV = 3;
            public const int GW_OWNER = 4;
            public const int GW_CHILD = 5;
            public const int GW_MAX = 5;

            #endregion Constants

            #region Delegates

            public delegate bool EnumProc(IntPtr hwnd, int lParam);

            public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

            public delegate IntPtr WndProc(IntPtr hWnd, MessageType msg, IntPtr wParam, IntPtr lParam);

            #endregion Delegates

            #region Functions

            [DllImport("user32.dll")]
            public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

            [DllImport("user32.dll")]
            public static extern bool GetClientRect(IntPtr hwnd, out RECT lpRect);

            [DllImport("user32.dll", SetLastError=true)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

            public static bool SetWindowPos(IntPtr hWnd, SetWindowPosHWndInsertAfterOption hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags) {
                return SetWindowPos(hWnd, (IntPtr) hWndInsertAfter, X, Y, cx, cy, uFlags);
            }

            [DllImport("user32.dll", SetLastError=true)]
            public static extern bool SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            [DllImport("user32.dll")]
            public static extern IntPtr SetCapture(IntPtr hwnd);

            [DllImport("user32.dll")]
            public static extern bool ReleaseCapture();

            [DllImport("user32.dll")]
            public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

            [DllImport("user32.dll")]
            public static extern IntPtr GetFocus();

            [DllImport("user32.dll")]
            public static extern IntPtr SetFocus(IntPtr hWnd);

            //GetWindowLong won't work correctly for 64-bit: we should use GetWindowLongPtr instead.  On
            //32-bit, GetWindowLongPtr is just #defined as GetWindowLong.  GetWindowLong really should
            //take/return int instead of IntPtr/IntPtr, but since we're running this only for 32-bit
            //it'll be OK.
            public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex) {
                if (IntPtr.Size == 4)
                    return GetWindowLong32(hWnd, nIndex);

                return GetWindowLongPtr64(hWnd, nIndex);
            }

            public static long GetWindowLongInteger(IntPtr hWnd, int nIndex) {
                if (IntPtr.Size == 4)
                    return GetWindowLong32Integer(hWnd, nIndex);

                return GetWindowLongPtr64Integer(hWnd, nIndex);
            }

            public static IntPtr GetWindowLong(IntPtr hWnd, GetWindowLongParameter nIndex) {
                return GetWindowLong(hWnd, (int) nIndex);
            }

            public static long GetWindowLongInteger(IntPtr hWnd, GetWindowLongParameter nIndex) {
                return GetWindowLongInteger(hWnd, (int) nIndex);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
            private static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtr")]
            private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
            private static extern int GetWindowLong32Integer(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtr")]
            private static extern long GetWindowLongPtr64Integer(IntPtr hWnd, int nIndex);

            //SetWindowLong won't work correctly for 64-bit: we should use SetWindowLongPtr instead.  On
            //32-bit, SetWindowLongPtr is just #defined as SetWindowLong.  SetWindowLong really should
            //take/return int instead of IntPtr/IntPtr, but since we're running this only for 32-bit
            //it'll be OK.
            public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong) {
                if (IntPtr.Size == 4) {
                    return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
                }
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }

            public static IntPtr SetWindowLong(IntPtr hWnd, GetWindowLongParameter nIndex, IntPtr wndproc) {
                return SetWindowLong(hWnd, (int) nIndex, wndproc);
            }

            public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong) {
                if (IntPtr.Size == 4) {
                    return SetWindowLongPtr32(hWnd, nIndex, (int) dwNewLong);
                }
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }

            public static IntPtr SetWindowLong(IntPtr hWnd, GetWindowLongParameter nIndex, long dwNewLong) {
                return SetWindowLong(hWnd, (int) nIndex, dwNewLong);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
            private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
            private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, long dwNewLong);

            public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, WndProc wndproc) {
                if (IntPtr.Size == 4)
                    return SetWindowLongPtr32(hWnd, nIndex, wndproc);

                return SetWindowLongPtr64(hWnd, nIndex, wndproc);
            }

            public static IntPtr SetWindowLong(IntPtr hWnd, GetWindowLongParameter nIndex, WndProc wndproc) {
                return SetWindowLong(hWnd, (int) nIndex, wndproc);
            }



            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
            private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
            private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
            private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, WndProc wndproc);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
            private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, WndProc wndproc);

            //GetClassLong won't work correctly for 64-bit: we should use GetClassLongPtr instead.  On
            //32-bit, GetClassLongPtr is just #defined as GetClassLong.  GetClassLong really should
            //take/return int instead of IntPtr/IntPtr, but since we're running this only for 32-bit
            //it'll be OK.
            public static IntPtr GetClassLong(IntPtr hWnd, int nIndex) {
                if (IntPtr.Size == 4)
                    return GetClassLong32(hWnd, nIndex);

                return GetClassLongPtr64(hWnd, nIndex);
            }

            public static long GetClassLongInteger(IntPtr hWnd, int nIndex) {
                if (IntPtr.Size == 4)
                    return GetClassLong32Integer(hWnd, nIndex);

                return GetClassLongPtr64Integer(hWnd, nIndex);
            }

            public static IntPtr GetClassLong(IntPtr hWnd, GetClassLongParameter nIndex) {
                return GetClassLong(hWnd, (int) nIndex);
            }

            public static long GetClassLongInteger(IntPtr hWnd, GetClassLongParameter nIndex) {
                return GetClassLongInteger(hWnd, (int) nIndex);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClassLong")]
            private static extern IntPtr GetClassLong32(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClassLongPtr")]
            private static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClassLong")]
            private static extern int GetClassLong32Integer(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClassLongPtr")]
            private static extern long GetClassLongPtr64Integer(IntPtr hWnd, int nIndex);

            //SetClassLong won't work correctly for 64-bit: we should use SetClassLongPtr instead.  On
            //32-bit, SetClassLongPtr is just #defined as SetClassLong.  SetClassLong really should
            //take/return int instead of IntPtr/IntPtr, but since we're running this only for 32-bit
            //it'll be OK.
            public static IntPtr SetClassLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong) {
                if (IntPtr.Size == 4) {
                    return SetClassLongPtr32(hWnd, nIndex, dwNewLong);
                }
                return SetClassLongPtr64(hWnd, nIndex, dwNewLong);
            }

            public static IntPtr SetClassLong(IntPtr hWnd, GetClassLongParameter nIndex, IntPtr wndproc) {
                return SetClassLong(hWnd, (int) nIndex, wndproc);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLong")]
            private static extern IntPtr SetClassLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLongPtr")]
            private static extern IntPtr SetClassLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            public static IntPtr SetClassLong(IntPtr hWnd, int nIndex, WndProc wndproc) {
                if (IntPtr.Size == 4) {
                    return SetClassLongPtr32(hWnd, nIndex, wndproc);
                }
                return SetClassLongPtr64(hWnd, nIndex, wndproc);
            }

            public static IntPtr SetClassLong(IntPtr hWnd, GetClassLongParameter nIndex, WndProc wndproc) {
                return SetClassLong(hWnd, (int) nIndex, wndproc);
            }


            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLong")]
            private static extern IntPtr SetClassLongPtr32(IntPtr hWnd, int nIndex, WndProc wndproc);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLongPtr")]
            private static extern IntPtr SetClassLongPtr64(IntPtr hWnd, int nIndex, WndProc wndproc);



            public static IntPtr SetClassLong(IntPtr hWnd, int nIndex, long dwNewLong) {
                if (IntPtr.Size == 4) {
                    return SetClassLongPtr32(hWnd, nIndex, (int) dwNewLong);
                }
                return SetClassLongPtr64(hWnd, nIndex, dwNewLong);
            }

            public static IntPtr SetClassLong(IntPtr hWnd, GetWindowLongParameter nIndex, long dwNewLong) {
                return SetClassLong(hWnd, (int) nIndex, dwNewLong);
            }

            public static IntPtr SetClassLong(IntPtr hWnd, GetClassLongParameter nIndex, long dwNewLong) {
                return SetClassLong(hWnd, (int) nIndex, dwNewLong);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLong")]
            private static extern IntPtr SetClassLongPtr32(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLongPtr")]
            private static extern IntPtr SetClassLongPtr64(IntPtr hWnd, int nIndex, long dwNewLong);



            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SetWindowsHookEx(HookType hook, HookProc callback, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll")]
            public static extern IntPtr UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll")]
            public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

            // [DllImport("user32.dll")]
            // public static extern IntPtr DefWindowProc(IntPtr hWnd, uint Msg, IntPtr wParam,IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern IntPtr DefDlgProc(IntPtr hDlg, uint Msg, IntPtr wParam, IntPtr lParam);

            // [DllImport("User32.dll")]
            // public static extern UIntPtr SetTimer(IntPtr hwnd, UIntPtr nIDEvent, uint uElapse, CallBack cbf);

            /// <summary>
            /// The CreateWindowEx function creates an overlapped, pop-up, or child window with an extended window style; otherwise, this function is identical to the CreateWindow function.
            /// </summary>
            /// <param name="dwExStyle">Specifies the extended window style of the window being created.</param>
            /// <param name="lpClassName">Pointer to a null-terminated string or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the high-order word must be zero. If lpClassName is a string, it specifies the window class name. The class name can be any name registered with RegisterClass or RegisterClassEx, provided that the module that registers the class is also the module that creates the window. The class name can also be any of the predefined system class names.</param>
            /// <param name="lpWindowName">Pointer to a null-terminated string that specifies the window name. If the window style specifies a title bar, the window title pointed to by lpWindowName is displayed in the title bar. When using CreateWindow to create controls, such as buttons, check boxes, and static controls, use lpWindowName to specify the text of the control. When creating a static control with the SS_ICON style, use lpWindowName to specify the icon name or identifier. To specify an identifier, use the syntax "#num". </param>
            /// <param name="dwStyle">Specifies the style of the window being created. This parameter can be a combination of window styles, plus the control styles indicated in the Remarks section.</param>
            /// <param name="x">Specifies the initial horizontal position of the window. For an overlapped or pop-up window, the x parameter is the initial x-coordinate of the window's upper-left corner, in screen coordinates. For a child window, x is the x-coordinate of the upper-left corner of the window relative to the upper-left corner of the parent window's client area. If x is set to CW_USEDEFAULT, the system selects the default position for the window's upper-left corner and ignores the y parameter. CW_USEDEFAULT is valid only for overlapped windows; if it is specified for a pop-up or child window, the x and y parameters are set to zero.</param>
            /// <param name="y">Specifies the initial vertical position of the window. For an overlapped or pop-up window, the y parameter is the initial y-coordinate of the window's upper-left corner, in screen coordinates. For a child window, y is the initial y-coordinate of the upper-left corner of the child window relative to the upper-left corner of the parent window's client area. For a list box y is the initial y-coordinate of the upper-left corner of the list box's client area relative to the upper-left corner of the parent window's client area.
            /// <para>If an overlapped window is created with the WS_VISIBLE style bit set and the x parameter is set to CW_USEDEFAULT, then the y parameter determines how the window is shown. If the y parameter is CW_USEDEFAULT, then the window manager calls ShowWindow with the SW_SHOW flag after the window has been created. If the y parameter is some other value, then the window manager calls ShowWindow with that value as the nCmdShow parameter.</para></param>
            /// <param name="nWidth">Specifies the width, in device units, of the window. For overlapped windows, nWidth is the window's width, in screen coordinates, or CW_USEDEFAULT. If nWidth is CW_USEDEFAULT, the system selects a default width and height for the window; the default width extends from the initial x-coordinates to the right edge of the screen; the default height extends from the initial y-coordinate to the top of the icon area. CW_USEDEFAULT is valid only for overlapped windows; if CW_USEDEFAULT is specified for a pop-up or child window, the nWidth and nHeight parameter are set to zero.</param>
            /// <param name="nHeight">Specifies the height, in device units, of the window. For overlapped windows, nHeight is the window's height, in screen coordinates. If the nWidth parameter is set to CW_USEDEFAULT, the system ignores nHeight.</param> <param name="hWndParent">Handle to the parent or owner window of the window being created. To create a child window or an owned window, supply a valid window handle. This parameter is optional for pop-up windows.
            /// <para>Windows 2000/XP: To create a message-only window, supply HWND_MESSAGE or a handle to an existing message-only window.</para></param>
            /// <param name="hMenu">Handle to a menu, or specifies a child-window identifier, depending on the window style. For an overlapped or pop-up window, hMenu identifies the menu to be used with the window; it can be NULL if the class menu is to be used. For a child window, hMenu specifies the child-window identifier, an integer value used by a dialog box control to notify its parent about events. The application determines the child-window identifier; it must be unique for all child windows with the same parent window.</param>
            /// <param name="hInstance">Handle to the instance of the module to be associated with the window.</param> <param name="lpParam">Pointer to a value to be passed to the window through the CREATESTRUCT structure (lpCreateParams member) pointed to by the lParam param of the WM_CREATE message. This message is sent to the created window by this function before it returns.
            /// <para>If an application calls CreateWindow to create a MDI client window, lpParam should point to a CLIENTCREATESTRUCT structure. If an MDI client window calls CreateWindow to create an MDI child window, lpParam should point to a MDICREATESTRUCT structure. lpParam may be NULL if no additional data is needed.</para></param>
            /// <returns>If the function succeeds, the return value is a handle to the new window.
            /// <para>If the function fails, the return value is NULL. To get extended error information, call GetLastError.</para>
            /// <para>This function typically fails for one of the following reasons:</para>
            /// <list type="">
            /// <item>an invalid parameter value</item>
            /// <item>the system class was registered by a different module</item>
            /// <item>The WH_CBT hook is installed and returns a failure code</item>
            /// <item>if one of the controls in the dialog template is not registered, or its window window procedure fails WM_CREATE or WM_NCCREATE</item>
            /// </list></returns>
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr CreateWindowEx(
                WindowStylesEx dwExStyle,
                string lpClassName,
                string lpWindowName,
                WindowStyles dwStyle,
                int x,
                int y,
                int nWidth,
                int nHeight,
                IntPtr hWndParent,
                IntPtr hMenu,
                IntPtr hInstance,
                IntPtr lpParam);

            [DllImport("user32.dll")]
            public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

            [DllImport("user32.dll")]
            public static extern int SetWindowText(IntPtr hwnd, string str);

            [DllImport("User32.dll")]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

            [DllImport("User32.Dll")]
            public static extern void GetClassName(IntPtr hwnd, StringBuilder s, int nMaxCount);

            [DllImport("user32.dll")]
            public static extern int FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            public static extern int FindWindowEx(IntPtr parent_h, IntPtr child_h, string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.U2)]
            public static extern short RegisterClassEx([In] ref WNDCLASSEX lpwcx);

            [DllImport("user32.dll")]
            public static extern int GetWindow(IntPtr hwnd, int flag);

            [DllImport("User32.dll")]
            public static extern void PostQuitMessage(int nExitCode);

            [DllImport("User32.dll")]
            public static extern IntPtr SendMessage(IntPtr hwnd, MessageType Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("User32.dll")]
            public static extern IntPtr PostMessage(IntPtr hwnd, MessageType Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern bool TranslateMessage([In] ref MSG lpMsg);

            [DllImport("user32.dll")]
            public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

            [DllImport("user32.dll")]
            public static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

            [DllImport("user32.dll")]
            public static extern int PeekMessage(
                out MSG lpMsg,
                IntPtr hWnd,
                uint wMsgFilterMin,
                uint wMsgFilterMax,
                PeekMessageOptions wRemoveMsg);

            [DllImport("user32.dll")]
            public static extern IntPtr DefWindowProc(IntPtr hWnd, MessageType uMsg, IntPtr wParam, IntPtr lParam);

            [DllImport("User32.dll")]
            public static extern bool RedrawWindow(IntPtr hwnd, ref RECT rcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);

            [DllImport("User32.dll")]
            public static extern bool RedrawWindow(IntPtr hwnd, IntPtr rcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);

            [DllImport("user32.dll")]
            public static extern IntPtr GetActiveWindow();

            [DllImport("User32.dll")]
            public static extern IntPtr SetActiveWindow(IntPtr hwnd);

            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("User32.dll")]
            public static extern int SetForegroundWindow(IntPtr hwnd);

            [DllImport("User32.dll")]
            public static extern int ShowWindow(IntPtr hwnd, ShowWindowCommands nCmdShow);

            [DllImport("user32")]
            public static extern int EnumWindows(EnumProc cbf, int lParam);

            [DllImport("user32")]
            public static extern int EnumChildWindows(IntPtr hwnd, EnumProc cbf, int lParam);

            [DllImport("user32.dll")]
            public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);

            [DllImport("User32.dll", EntryPoint = "EnumThreadWindows")]
            public static extern bool EnumThreadWindows(uint dwThreadId, EnumProc cbf, int lParam);

            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern bool UpdateLayeredWindow(
                IntPtr hwnd,
                IntPtr hdcDst,
                ref POINT pptDst,
                ref SIZE psize,
                IntPtr hdcSrc,
                ref POINT pptSrc,
                uint crKey,
                [In] ref BLENDFUNCTION pblend,
                uint dwFlags);

            [DllImport("user32.dll")]
            public static extern IntPtr LoadIcon(IntPtr hInstance, string lpIconName);

            [DllImport("user32.dll")]
            private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

            public static IntPtr LoadIcon(IconId lpIconName) {
                return LoadIcon(IntPtr.Zero, new IntPtr((int) lpIconName));
            }

            [DllImport("user32.dll")]
            public static extern IntPtr LoadCursor(IntPtr hInstance, string lpCursorName);

            [DllImport("user32.dll")]
            private static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);

            public static IntPtr LoadCursor(CursorId lpCursorName) {
                return LoadCursor(IntPtr.Zero, new IntPtr((int) lpCursorName));
            }

            #endregion Functions

            #region Enums

            public enum IconId : int {
                IDI_APPLICATION = 32512,
                IDI_HAND = 32513,
                IDI_QUESTION = 32514,
                IDI_EXCLAMATION = 32515,
                IDI_ASTERISK = 32516,
                IDI_WINLOGO = 32517,
                IDI_SHIELD = 32518,
                IDI_WARNING = IDI_EXCLAMATION,
                IDI_ERROR = IDI_HAND,
                IDI_INFORMATION = IDI_ASTERISK,
            }

            public enum CursorId : int {
                IDC_ARROW = 32512,
                IDC_IBEAM = 32513,
                IDC_WAIT = 32514,
                IDC_CROSS = 32515,
                IDC_UPARROW = 32516,

                [Obsolete("use IDC_SIZEALL")]
                IDC_SIZE = 32640,

                [Obsolete("use IDC_ARROW")]
                IDC_ICON = 32641,
                IDC_SIZENWSE = 32642,
                IDC_SIZENESW = 32643,
                IDC_SIZEWE = 32644,
                IDC_SIZENS = 32645,
                IDC_SIZEALL = 32646,
                IDC_NO = 32648,
                IDC_HAND = 32649,
                IDC_APPSTARTING = 32650,
                IDC_HELP = 32651
            }

            public enum HitTestResult {
                HTERROR = (-2),
                HTTRANSPARENT = (-1),
                HTNOWHERE = 0,
                HTCLIENT = 1,
                HTCAPTION = 2,
                HTSYSMENU = 3,
                HTGROWBOX = 4,
                HTSIZE = HTGROWBOX,
                HTMENU = 5,
                HTHSCROLL = 6,
                HTVSCROLL = 7,
                HTMINBUTTON = 8,
                HTMAXBUTTON = 9,
                HTLEFT = 10,
                HTRIGHT = 11,
                HTTOP = 12,
                HTTOPLEFT = 13,
                HTTOPRIGHT = 14,
                HTBOTTOM = 15,
                HTBOTTOMLEFT = 16,
                HTBOTTOMRIGHT = 17,
                HTBORDER = 18,
                HTREDUCE = HTMINBUTTON,
                HTZOOM = HTMAXBUTTON,
                HTSIZEFIRST = HTLEFT,
                HTSIZELAST = HTBOTTOMRIGHT,
                HTOBJECT = 19,
                HTCLOSE = 20,
                HTHELP = 21,
            }

            // Queue status flags for GetQueueStatus() and MsgWaitForMultipleObjects()
            public enum QueueStatusFlags : uint {
                QS_KEY = 0x0001,
                QS_MOUSEMOVE = 0x0002,
                QS_MOUSEBUTTON = 0x0004,
                QS_POSTMESSAGE = 0x0008,
                QS_TIMER = 0x0010,
                QS_PAINT = 0x0020,
                QS_SENDMESSAGE = 0x0040,
                QS_HOTKEY = 0x0080,
                QS_ALLPOSTMESSAGE = 0x0100,

                QS_RAWINPUT = 0x0400,

                QS_TOUCH = 0x0800,
                QS_POINTER = 0x1000,

                QS_MOUSE = QS_MOUSEMOVE | QS_MOUSEBUTTON,

                QS_INPUT = QS_MOUSE | QS_KEY | QS_RAWINPUT | QS_TOUCH | QS_POINTER,

                QS_ALLEVENTS = QS_INPUT | QS_POSTMESSAGE | QS_TIMER | QS_PAINT | QS_HOTKEY,

                QS_ALLINPUT = QS_INPUT | QS_POSTMESSAGE | QS_TIMER | QS_PAINT | QS_HOTKEY | QS_SENDMESSAGE,
            }

            public enum ShowWindowCommands : int {
                SW_HIDE = 0,
                SW_SHOWNORMAL = 1,
                SW_NORMAL = 1,
                SW_SHOWMINIMIZED = 2,
                SW_SHOWMAXIMIZED = 3,
                SW_MAXIMIZE = 3,
                SW_SHOWNOACTIVATE = 4,
                SW_SHOW = 5,
                SW_MINIMIZE = 6,
                SW_SHOWMINNOACTIVE = 7,
                SW_SHOWNA = 8,
                SW_RESTORE = 9,
                SW_SHOWDEFAULT = 10,
                SW_FORCEMINIMIZE = 11,
                SW_MAX = 11,
            }

            public enum PeekMessageOptions : uint {
                PM_NOREMOVE = 0x0000,
                PM_REMOVE = 0x0001,
                PM_NOYIELD = 0x0002,
                PM_QS_INPUT = QueueStatusFlags.QS_INPUT << 16,
                PM_QS_POSTMESSAGE = (QueueStatusFlags.QS_POSTMESSAGE | QueueStatusFlags.QS_HOTKEY | QueueStatusFlags.QS_TIMER) << 16,
                PM_QS_PAINT = QueueStatusFlags.QS_PAINT << 16,
                PM_QS_SENDMESSAGE = QueueStatusFlags.QS_SENDMESSAGE << 16,
            }

            public enum CreateWindowParameters : int {
                CW_USEDEFAULT = unchecked ((int) 0x80000000)
            }

            public enum HookType : int {
                WH_JOURNALRECORD = 0,
                WH_JOURNALPLAYBACK = 1,
                WH_KEYBOARD = 2,
                WH_GETMESSAGE = 3,
                WH_CALLWNDPROC = 4,
                WH_CBT = 5,
                WH_SYSMSGFILTER = 6,
                WH_MOUSE = 7,
                WH_HARDWARE = 8,
                WH_DEBUG = 9,
                WH_SHELL = 10,
                WH_FOREGROUNDIDLE = 11,
                WH_CALLWNDPROCRET = 12,
                WH_KEYBOARD_LL = 13,
                WH_MOUSE_LL = 14
            }

            public enum RedrawWindowFlags {
                RDW_INVALIDATE = 0x0001,
                RDW_INTERNALPAINT = 0x0002,
                RDW_ERASE = 0x0004,

                RDW_VALIDATE = 0x0008,
                RDW_NOINTERNALPAINT = 0x0010,
                RDW_NOERASE = 0x0020,

                RDW_NOCHILDREN = 0x0040,
                RDW_ALLCHILDREN = 0x0080,

                RDW_UPDATENOW = 0x0100,
                RDW_ERASENOW = 0x0200,

                RDW_FRAME = 0x0400,
                RDW_NOFRAME = 0x0800
            }

            [Flags]
            public enum WindowStylesEx : uint {
                /// <summary>Specifies a window that accepts drag-drop files.</summary>
                WS_EX_ACCEPTFILES = 0x00000010,

                /// <summary>Forces a top-level window onto the taskbar when the window is visible.</summary>
                WS_EX_APPWINDOW = 0x00040000,

                /// <summary>Specifies a window that has a border with a sunken edge.</summary>
                WS_EX_CLIENTEDGE = 0x00000200,

                /// <summary>
                /// Specifies a window that paints all descendants in bottom-to-top painting order using double-buffering.
                /// This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. This style is not supported in Windows 2000.
                /// </summary>
                /// <remarks>
                /// With WS_EX_COMPOSITED set, all descendants of a window get bottom-to-top painting order using double-buffering.
                /// Bottom-to-top painting order allows a descendent window to have translucency (alpha) and transparency (color-key) effects,
                /// but only if the descendent window also has the WS_EX_TRANSPARENT bit set.
                /// Double-buffering allows the window and its descendents to be painted without flicker.
                /// </remarks>
                WS_EX_COMPOSITED = 0x02000000,

                /// <summary>
                /// Specifies a window that includes a question mark in the title bar. When the user clicks the question mark,
                /// the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message.
                /// The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command.
                /// The Help application displays a pop-up window that typically contains help for the child window.
                /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
                /// </summary>
                WS_EX_CONTEXTHELP = 0x00000400,

                /// <summary>
                /// Specifies a window which contains child windows that should take part in dialog box navigation.
                /// If this style is specified, the dialog manager recurses into children of this window when performing navigation operations
                /// such as handling the TAB key, an arrow key, or a keyboard mnemonic.
                /// </summary>
                WS_EX_CONTROLPARENT = 0x00010000,

                /// <summary>Specifies a window that has a double border.</summary>
                WS_EX_DLGMODALFRAME = 0x00000001,

                /// <summary>
                /// Specifies a window that is a layered window.
                /// This cannot be used for child windows or if the window has a class style of either CS_OWNDC or CS_CLASSDC.
                /// </summary>
                WS_EX_LAYERED = 0x00080000,

                /// <summary>
                /// Specifies a window with the horizontal origin on the right edge. Increasing horizontal values advance to the left.
                /// The shell language must support reading-order alignment for this to take effect.
                /// </summary>
                WS_EX_LAYOUTRTL = 0x00400000,

                /// <summary>Specifies a window that has generic left-aligned properties. This is the default.</summary>
                WS_EX_LEFT = 0x00000000,

                /// <summary>
                /// Specifies a window with the vertical scroll bar (if present) to the left of the client area.
                /// The shell language must support reading-order alignment for this to take effect.
                /// </summary>
                WS_EX_LEFTSCROLLBAR = 0x00004000,

                /// <summary>
                /// Specifies a window that displays text using left-to-right reading-order properties. This is the default.
                /// </summary>
                WS_EX_LTRREADING = 0x00000000,

                /// <summary>
                /// Specifies a multiple-document interface (MDI) child window.
                /// </summary>
                WS_EX_MDICHILD = 0x00000040,

                /// <summary>
                /// Specifies a top-level window created with this style does not become the foreground window when the user clicks it.
                /// The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
                /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
                /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
                /// </summary>
                WS_EX_NOACTIVATE = 0x08000000,

                /// <summary>
                /// Specifies a window which does not pass its window layout to its child windows.
                /// </summary>
                WS_EX_NOINHERITLAYOUT = 0x00100000,

                /// <summary>
                /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
                /// </summary>
                WS_EX_NOPARENTNOTIFY = 0x00000004,

                /// <summary>Specifies an overlapped window.</summary>
                WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,

                /// <summary>Specifies a palette window, which is a modeless dialog box that presents an array of commands.</summary>
                WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,

                /// <summary>
                /// Specifies a window that has generic "right-aligned" properties. This depends on the window class.
                /// The shell language must support reading-order alignment for this to take effect.
                /// Using the WS_EX_RIGHT style has the same effect as using the SS_RIGHT (static), ES_RIGHT (edit), and BS_RIGHT/BS_RIGHTBUTTON (button) control styles.
                /// </summary>
                WS_EX_RIGHT = 0x00001000,

                /// <summary>Specifies a window with the vertical scroll bar (if present) to the right of the client area. This is the default.</summary>
                WS_EX_RIGHTSCROLLBAR = 0x00000000,

                /// <summary>
                /// Specifies a window that displays text using right-to-left reading-order properties.
                /// The shell language must support reading-order alignment for this to take effect.
                /// </summary>
                WS_EX_RTLREADING = 0x00002000,

                /// <summary>Specifies a window with a three-dimensional border style intended to be used for items that do not accept user input.</summary>
                WS_EX_STATICEDGE = 0x00020000,

                /// <summary>
                /// Specifies a window that is intended to be used as a floating toolbar.
                /// A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font.
                /// A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB.
                /// If a tool window has a system menu, its icon is not displayed on the title bar.
                /// However, you can display the system menu by right-clicking or by typing ALT+SPACE.
                /// </summary>
                WS_EX_TOOLWINDOW = 0x00000080,

                /// <summary>
                /// Specifies a window that should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
                /// To add or remove this style, use the SetWindowPos function.
                /// </summary>
                WS_EX_TOPMOST = 0x00000008,

                /// <summary>
                /// Specifies a window that should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
                /// The window appears transparent because the bits of underlying sibling windows have already been painted.
                /// To achieve transparency without these restrictions, use the SetWindowRgn function.
                /// </summary>
                WS_EX_TRANSPARENT = 0x00000020,

                /// <summary>Specifies a window that has a border with a raised edge.</summary>
                WS_EX_WINDOWEDGE = 0x00000100
            }

            /// <summary>
            /// Window Styles.
            /// The following styles can be specified wherever a window style is required. After the control has been created, these styles cannot be modified, except as noted.
            /// </summary>
            [Flags]
            public enum WindowStyles : uint {
                /// <summary>The window has a thin-line border.</summary>
                WS_BORDER = 0x800000,

                /// <summary>The window has a title bar (includes the WS_BORDER style).</summary>
                WS_CAPTION = 0xc00000,

                /// <summary>The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.</summary>
                WS_CHILD = 0x40000000,

                /// <summary>Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.</summary>
                WS_CLIPCHILDREN = 0x2000000,

                /// <summary>
                /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated.
                /// If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
                /// </summary>
                WS_CLIPSIBLINGS = 0x4000000,

                /// <summary>The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.</summary>
                WS_DISABLED = 0x8000000,

                /// <summary>The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.</summary>
                WS_DLGFRAME = 0x400000,

                /// <summary>
                /// The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style.
                /// The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.
                /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
                /// </summary>
                WS_GROUP = 0x20000,

                /// <summary>The window has a horizontal scroll bar.</summary>
                WS_HSCROLL = 0x100000,

                /// <summary>The window is initially maximized.</summary>
                WS_MAXIMIZE = 0x1000000,

                /// <summary>The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
                WS_MAXIMIZEBOX = 0x10000,

                /// <summary>The window is initially minimized.</summary>
                WS_MINIMIZE = 0x20000000,

                /// <summary>The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
                WS_MINIMIZEBOX = 0x20000,

                /// <summary>The window is an overlapped window. An overlapped window has a title bar and a border.</summary>
                WS_OVERLAPPED = 0x0,

                /// <summary>The window is an overlapped window.</summary>
                WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

                /// <summary>The window is a pop-up window. This style cannot be used with the WS_CHILD style.</summary>
                WS_POPUP = 0x80000000,

                /// <summary>The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.</summary>
                WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,

                /// <summary>The window has a sizing border.</summary>
                WS_SIZEFRAME = 0x40000,

                /// <summary>The window has a window menu on its title bar. The WS_CAPTION style must also be specified.</summary>
                WS_SYSMENU = 0x80000,

                /// <summary>
                /// The window is a control that can receive the keyboard focus when the user presses the TAB key.
                /// Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.
                /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
                /// For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
                /// </summary>
                WS_TABSTOP = 0x10000,

                /// <summary>The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.</summary>
                WS_VISIBLE = 0x10000000,

                /// <summary>The window has a vertical scroll bar.</summary>
                WS_VSCROLL = 0x200000
            }

            public enum SystemColorId : uint {
                CTLCOLOR_MSGBOX = 0,
                CTLCOLOR_EDIT = 1,
                CTLCOLOR_LISTBOX = 2,
                CTLCOLOR_BTN = 3,
                CTLCOLOR_DLG = 4,
                CTLCOLOR_SCROLLBAR = 5,
                CTLCOLOR_STATIC = 6,
                CTLCOLOR_MAX = 7,

                COLOR_SCROLLBAR = 0,
                COLOR_BACKGROUND = 1,
                COLOR_ACTIVECAPTION = 2,
                COLOR_INACTIVECAPTION = 3,
                COLOR_MENU = 4,
                COLOR_WINDOW = 5,
                COLOR_WINDOWFRAME = 6,
                COLOR_MENUTEXT = 7,
                COLOR_WINDOWTEXT = 8,
                COLOR_CAPTIONTEXT = 9,
                COLOR_ACTIVEBORDER = 10,
                COLOR_INACTIVEBORDER = 11,
                COLOR_APPWORKSPACE = 12,
                COLOR_HIGHLIGHT = 13,
                COLOR_HIGHLIGHTTEXT = 14,
                COLOR_BTNFACE = 15,
                COLOR_BTNSHADOW = 16,
                COLOR_GRAYTEXT = 17,
                COLOR_BTNTEXT = 18,
                COLOR_INACTIVECAPTIONTEXT = 19,
                COLOR_BTNHIGHLIGHT = 20,

                COLOR_3DDKSHADOW = 21,
                COLOR_3DLIGHT = 22,
                COLOR_INFOTEXT = 23,
                COLOR_INFOBK = 24,

                COLOR_HOTLIGHT = 26,
                COLOR_GRADIENTACTIVECAPTION = 27,
                COLOR_GRADIENTINACTIVECAPTION = 28,
                COLOR_MENUHILIGHT = 29,
                COLOR_MENUBAR = 30,

                COLOR_DESKTOP = COLOR_BACKGROUND,
                COLOR_3DFACE = COLOR_BTNFACE,
                COLOR_3DSHADOW = COLOR_BTNSHADOW,
                COLOR_3DHIGHLIGHT = COLOR_BTNHIGHLIGHT,
                COLOR_3DHILIGHT = COLOR_BTNHIGHLIGHT,
                COLOR_BTNHILIGHT = COLOR_BTNHIGHLIGHT,
            }

            [Flags]
            public enum SetWindowPosHWndInsertAfterOption : int {
                HWND_TOP = 0,
                HWND_BOTTOM = 1,
                HWND_TOPMOST = -1,
                HWND_NOTOPMOST = -2,
            }

            [Flags]
            public enum SetWindowPosFlags : int {
                SWP_NOSIZE = 0x0001,
                SWP_NOMOVE = 0x0002,
                SWP_NOZORDER = 0x0004,
                SWP_NOREDRAW = 0x0008,
                SWP_NOACTIVATE = 0x0010,
                SWP_FRAMECHANGED = 0x0020, /* The frame changed: send WM_NCCALCSIZE */
                SWP_SHOWWINDOW = 0x0040,
                SWP_HIDEWINDOW = 0x0080,
                SWP_NOCOPYBITS = 0x0100,
                SWP_NOOWNERZORDER = 0x0200, /* Don't do owner Z ordering */
                SWP_NOSENDCHANGING = 0x0400, /* Don't send WM_WINDOWPOSCHANGING */

                SWP_DRAWFRAME = SWP_FRAMECHANGED,
                SWP_NOREPOSITION = SWP_NOOWNERZORDER
            }

            [Flags]
            public enum WindowClassStyles : uint {
                /// <summary>Aligns the window's client area on a byte boundary (in the x direction). This style affects the width of the window and its horizontal placement on the display.</summary>
                CS_BYTEALIGNCLIENT = 0x1000,

                /// <summary>Aligns the window on a byte boundary (in the x direction). This style affects the width of the window and its horizontal placement on the display.</summary>
                CS_BYTEALIGNWINDOW = 0x2000,

                /// <summary>
                /// Allocates one device context to be shared by all windows in the class.
                /// Because window classes are process specific, it is possible for multiple threads of an application to create a window of the same class.
                /// It is also possible for the threads to attempt to use the device context simultaneously. When this happens, the system allows only one thread to successfully finish its drawing operation.
                /// </summary>
                CS_CLASSDC = 0x40,

                /// <summary>Sends a double-click message to the window procedure when the user double-clicks the mouse while the cursor is within a window belonging to the class.</summary>
                CS_DBLCLKS = 0x8,

                /// <summary>
                /// Enables the drop shadow effect on a window. The effect is turned on and off through SPI_SETDROPSHADOW.
                /// Typically, this is enabled for small, short-lived windows such as menus to emphasize their Z order relationship to other windows.
                /// </summary>
                CS_DROPSHADOW = 0x20000,

                /// <summary>Indicates that the window class is an application global class. For more information, see the "Application Global Classes" section of About Window Classes.</summary>
                CS_GLOBALCLASS = 0x4000,

                /// <summary>Redraws the entire window if a movement or size adjustment changes the width of the client area.</summary>
                CS_HREDRAW = 0x2,

                /// <summary>Disables Close on the window menu.</summary>
                CS_NOCLOSE = 0x200,

                /// <summary>Allocates a unique device context for each window in the class.</summary>
                CS_OWNDC = 0x20,

                /// <summary>
                /// Sets the clipping rectangle of the child window to that of the parent window so that the child can draw on the parent.
                /// A window with the CS_PARENTDC style bit receives a regular device context from the system's cache of device contexts.
                /// It does not give the child the parent's device context or device context settings. Specifying CS_PARENTDC enhances an application's performance.
                /// </summary>
                CS_PARENTDC = 0x80,

                /// <summary>
                /// Saves, as a bitmap, the portion of the screen image obscured by a window of this class.
                /// When the window is removed, the system uses the saved bitmap to restore the screen image, including other windows that were obscured.
                /// Therefore, the system does not send WM_PAINT messages to windows that were obscured if the memory used by the bitmap has not been discarded and if other screen actions have not invalidated the stored image.
                /// This style is useful for small windows (for example, menus or dialog boxes) that are displayed briefly and then removed before other screen activity takes place.
                /// This style increases the time required to display the window, because the system must first allocate memory to store the bitmap.
                /// </summary>
                CS_SAVEBITS = 0x800,

                /// <summary>Redraws the entire window if a movement or size adjustment changes the height of the client area.</summary>
                CS_VREDRAW = 0x1
            }

            public enum GetWindowLongParameter : int {
                GWL_WNDPROC = -4,
                GWL_HINSTANCE = -6,
                GWL_HWNDPARENT = -8,
                GWL_STYLE = -16,
                GWL_EXSTYLE = -20,
                GWL_USERDATA = -21,
                GWL_ID = -12,
            }

            public enum WindowIconType : int {
                ICON_SMALL = 0,
                ICON_BIG = 1,
                ICON_SMALL2 = 2
            }

            public enum GetClassLongParameter : int {
                GCL_MENUNAME = -8,
                GCL_HBRBACKGROUND = -10,
                GCL_HCURSOR = -12,
                GCL_HICON = -14,
                GCL_HMODULE = -16,
                GCL_CBWNDEXTRA = -18,
                GCL_CBCLSEXTRA = -20,
                GCL_WNDPROC = -24,
                GCL_STYLE = -26,
                GCW_ATOM = -32,
                GCL_HICONSM = -34,
            }

            #endregion Enums

            #region Structs

            [StructLayout(LayoutKind.Sequential)]
            public struct WNDCLASSEX {
                public static readonly uint SizeOf = (uint) Marshal.SizeOf(typeof(WNDCLASSEX));

                public uint cbSize;
                public WindowClassStyles style;

                [MarshalAs(UnmanagedType.FunctionPtr)]
                public WndProc lpfnWndProc;

                public int cbClsExtra;
                public int cbWndExtra;
                public IntPtr hInstance;
                public IntPtr hIcon;
                public IntPtr hCursor;
                public IntPtr hbrBackground;
                public string lpszMenuName;
                public string lpszClassName;
                public IntPtr hIconSm;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct WNDCLASS {
                public WindowClassStyles style;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public WndProc lpfnWndProc;
                public int cbClsExtra;
                public int cbWndExtra;
                public IntPtr hInstance;
                public IntPtr hIcon;
                public IntPtr hCursor;
                public IntPtr hbrBackground;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string lpszMenuName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string lpszClassName;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CWPSTRUCT {
                public IntPtr lParam;
                public IntPtr wParam;
                public MessageType message;
                public IntPtr hwnd;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CWPRETSTRUCT {
                public IntPtr lResult;
                public IntPtr lParam;
                public IntPtr wParam;
                public MessageType message;
                public IntPtr hwnd;
            }

            #endregion Structs
        }
    }
}
