using System;
using LostPolygon.NanoWinForms;
using LostPolygon.WinApiWrapper;

public class BorderlessWindowBorderFilter : IMessageFilter {
    private Form _window;

    public int TopBorder { get; set; }
    public int LeftBorder { get; set; }
    public int BottomBorder { get; set; }
    public int RightBorder { get; set; }

    public BorderlessWindowBorderFilter(Form window) {
        _window = window;
        TopBorder = LeftBorder = BottomBorder = RightBorder = 16;
    }

    public void PostProcessMessage(ref WindowMessage message) {

    }

    public bool PreProcessMessage(ref WindowMessage message) {
        if (message.MessageType == WinApi.MessageType.WM_NCHITTEST) {
            WinApi.POINT cursorPosition = WinApi.DecodeUtility.DecodePoint((int) message.LParam);

            WinApi.User32.ScreenToClient(_window.Handle, ref cursorPosition);
            WinApi.User32.HitTestResult windowEdgeType = GetWindowEdgeType(_window.ClientRect, cursorPosition);

            message.Result = (IntPtr) windowEdgeType;
            return true;
        }

        return false;
    }

    private WinApi.User32.HitTestResult GetWindowEdgeType(WinApi.RECT rect, WinApi.POINT position) {
        //if (!rect.Contains(position))
        //    return BorderlessWindow.WindowEdgeType.Client;

        // Normalize coordinates
        position.X -= rect.X;
        position.Y -= rect.Y;

        rect.X = 0;
        rect.Y = 0;

        bool nearTopEdge = position.Y < TopBorder;
        bool nearLeftEdge = position.X < LeftBorder;
        bool nearRightEdge = position.X > rect.Width - RightBorder;
        bool nearBottomEdge = position.Y > rect.Height - BottomBorder;

        if (nearTopEdge) {
            if (nearLeftEdge)
                return WinApi.User32.HitTestResult.HTTOPLEFT;

            if (nearRightEdge)
                return  WinApi.User32.HitTestResult.HTTOPRIGHT;

            return  WinApi.User32.HitTestResult.HTTOP;
        }

        if (nearBottomEdge) {
            if (nearLeftEdge)
                return  WinApi.User32.HitTestResult.HTBOTTOMLEFT;

            if (nearRightEdge)
                return  WinApi.User32.HitTestResult.HTBOTTOMRIGHT;

            return  WinApi.User32.HitTestResult.HTBOTTOM;
        }

        if (nearLeftEdge)
            return  WinApi.User32.HitTestResult.HTLEFT;

        if (nearRightEdge)
            return  WinApi.User32.HitTestResult.HTRIGHT;

        return  WinApi.User32.HitTestResult.HTCLIENT;
    }
}