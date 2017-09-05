using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LostPolygon.NanoWinForms;
using LostPolygon.WinApiWrapper;

namespace LostPolygon.WinApiWrapperTest
{


    class Program {

    [DllImport("kernel32.dll")]
    public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    private const uint ENABLE_EXTENDED_FLAGS = 0x0080;


        private static WinApi.User32.WndProc _wndProc;

        static void Main(string[] args) {
         IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
         SetConsoleMode(handle, ENABLE_EXTENDED_FLAGS);



            const string appName = "Test App";
            const string wndName = "Test App Window";

            ApplicationContext applicationContext = new ApplicationContext();

            Form form1 = new Form(applicationContext);
            form1.Text = "ololo";
            form1.Text += "GG";
            WinApi.RECT rect = form1.Rect;
            rect.Width = 300;
            rect.Height = 200;
            form1.Rect = rect;
            form1.Show();
            form1.Text += "GG";
            //Form form2 = new Form(applicationContext);
            //form2.Text = "ololo";
            //form2.Text += "GG2";
            //rect = form2.Rect;
            //rect.Width = 500;
            //rect.Height = 400;
            //form2.Rect = rect;
            //form2.Show();


            applicationContext.Run();
            return;

            IntPtr hThisInst = WinApi.Kernel32.GetModuleHandle(null);
            _wndProc = WndProc;

            WinApi.User32.WNDCLASSEX wndClassEx = new WinApi.User32.WNDCLASSEX();
            wndClassEx.cbSize = WinApi.User32.WNDCLASSEX.SizeOf;
            wndClassEx.style = WinApi.User32.WindowClassStyles.CS_HREDRAW | WinApi.User32.WindowClassStyles.CS_VREDRAW;
            wndClassEx.lpfnWndProc = _wndProc;
            wndClassEx.cbClsExtra = 0;
            wndClassEx.cbWndExtra = 0;
            wndClassEx.hInstance = IntPtr.Zero;
            wndClassEx.hIcon = WinApi.User32.LoadIcon(WinApi.User32.IconId.IDI_SHIELD);
            wndClassEx.hIconSm = WinApi.User32.LoadIcon(WinApi.User32.IconId.IDI_SHIELD);
            wndClassEx.hCursor = WinApi.User32.LoadCursor(WinApi.User32.CursorId.IDC_ARROW);
            wndClassEx.hbrBackground = new IntPtr((int)WinApi.User32.SystemColorId.COLOR_WINDOW);
            wndClassEx.lpszClassName = "test app";

            if (WinApi.User32.RegisterClassEx(ref wndClassEx) == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            _hWnd = WinApi.User32.CreateWindowEx(
                WinApi.User32.WindowStylesEx.WS_EX_TOPMOST,
                "test app",
                wndName,
                WinApi.User32.WindowStyles.WS_VISIBLE | WinApi.User32.WindowStyles.WS_OVERLAPPEDWINDOW,
                200, 150,
                300, 200,
                IntPtr.Zero,
                IntPtr.Zero,
                hThisInst,
                IntPtr.Zero);

            if (_hWnd == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            WinApi.MSG message;

            bool isExit = false;
            //while (!isExit) {
            //while (WinApi.User32.PeekMessage(out message, IntPtr.Zero, 0, 0, WinApi.User32.PeekMessageOptions.PM_NOREMOVE)) {
            while (WinApi.User32.GetMessage(out message, _hWnd, 0, 0) > 0)
            {
                WinApi.User32.TranslateMessage(ref message);
                WinApi.User32.DispatchMessage(ref message);
            }
            //}

            Console.WriteLine("end");
            Console.ReadLine();
        }

        private static IntPtr _hWnd;

        private static  WinApi.RECT _downWndRect;
        private static  WinApi.POINT _downMousePos;
        private static bool _isDragging;

        private static void UpdatePos(WinApi.POINT currentPoint) {
            WinApi.POINT delta = new WinApi.POINT(_downMousePos.X - currentPoint.X, _downMousePos.Y - currentPoint.Y);

            WinApi.RECT windowRect = _downWndRect;
            windowRect.X = windowRect.X - delta.X;
            windowRect.Y = windowRect.Y - delta.Y;

            WinApi.User32.SetWindowPos(_hWnd, IntPtr.Zero, windowRect.X, windowRect.Y, windowRect.Width, windowRect.Height, 0);
        }

        private static IntPtr WndProc(IntPtr hWnd, WinApi.MessageType msg, IntPtr wParam, IntPtr lParam) {
            Console.WriteLine(msg);
            if (msg == WinApi.MessageType.WM_PAINT) {
                //Console.WriteLine(msg + " " + DateTime.Now);
            }

            if (msg == WinApi.MessageType.WM_MOVE) {
                //WinApi.User32.SendMessage(_hWnd, WinApi.MessageType.WM_PAINT, IntPtr.Zero, IntPtr.Zero);
            }

            switch (msg) {
                //case WinApi.MessageType.WM_NCHITTEST:
                //case WinApi.MessageType.HDM_HITTEST:
                //    return (IntPtr) WinApi.User32.HitTestResult.HTNOWHERE;
                //    WinApi.User32.HitTestResult hitTestResult = (WinApi.User32.HitTestResult) WinApi.User32.DefWindowProc(hWnd, msg, wParam, lParam);
                //    if (hitTestResult == WinApi.User32.HitTestResult.HTCLIENT) {
                //        return (IntPtr) WinApi.User32.HitTestResult.HTTRANSPARENT;
                //    }
                //    Console.WriteLine(hitTestResult);
                //    return (IntPtr) hitTestResult;
                //    break;
                ////case WinApi.MessageType.WM_NCLBUTTONDOWN:
                ////    if (wParam != (IntPtr) HT_CAPTION)
                ////        return WinApi.User32.DefWindowProc(hWnd, msg, wParam, lParam);

                ////    _isDragging = true;

                ////    WinApi.User32.SetCapture(hWnd);
                ////    WinApi.User32.GetWindowRect(_hWnd, out _downWndRect);
                ////    _downMousePos = GetCursorPos(lParam);

                ////    return IntPtr.Zero;
                //case WinApi.MessageType.WM_MOUSEMOVE:
                //    if (!_isDragging)
                //        return WinApi.User32.DefWindowProc(hWnd, msg, wParam, lParam);

                //    WinApi.POINT cursorPos = GetCursorPos(lParam);
                //    WinApi.User32.ClientToScreen(hWnd, ref cursorPos);
                //    UpdatePos(cursorPos);

                //    return IntPtr.Zero;
                //case WinApi.MessageType.WM_LBUTTONUP:
                //    if (_isDragging) {
                //        _isDragging = false;

                //        WinApi.User32.ReleaseCapture();
                //        return IntPtr.Zero;
                //    }

                //    return WinApi.User32.DefWindowProc(hWnd, msg, wParam, lParam);
                default:
                    return WinApi.User32.DefWindowProc(hWnd, msg, wParam, lParam);
            }
        }

        private static WinApi.POINT GetCursorPos(IntPtr lParam) {
            WinApi.POINT point;

            point.X = WinApi.BitUtility.LowWord((int) lParam);
            point.Y = WinApi.BitUtility.HighWord((int) lParam);
            return point;
        }
    }
}
