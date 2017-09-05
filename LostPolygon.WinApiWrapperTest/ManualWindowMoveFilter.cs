using System;
using LostPolygon.NanoWinForms;
using LostPolygon.WinApiWrapper;

internal class ManualWindowMoveFilter : IMessageFilter {
    private readonly Form _ownerForm;
    private bool _isDragging;
    private WinApi.POINT _downMousePos;
    private WinApi.RECT _downWndRect;

    public ManualWindowMoveFilter(Form form) {
        _ownerForm = form;
    }

    public bool PreProcessMessage(ref WindowMessage message) {
        //Debug.Log(message.MessageType);
        switch (message.MessageType) {
            case WinApi.MessageType.WM_NCLBUTTONDOWN:
                if (message.WParam != (IntPtr) WinApi.User32.HitTestResult.HTCAPTION)
                    return false;

                _isDragging = true;

                WinApi.User32.SetCapture(_ownerForm.Handle);
                WinApi.User32.GetWindowRect(_ownerForm.Handle, out _downWndRect);
                _downMousePos = WinApi.DecodeUtility.DecodePoint((int) message.LParam);

                message.Result = IntPtr.Zero;
                return true;
            case WinApi.MessageType.WM_MOUSEMOVE:
                if (!_isDragging)
                    return false;

                WinApi.POINT cursorPos = WinApi.DecodeUtility.DecodePoint((int) message.LParam);
                WinApi.User32.ClientToScreen(_ownerForm.Handle, ref cursorPos);
                UpdatePos(cursorPos);

                message.Result = IntPtr.Zero;
                return true;
            case WinApi.MessageType.WM_LBUTTONUP:
                if (_isDragging) {
                    _isDragging = false;

                    WinApi.User32.ReleaseCapture();
                    message.Result = IntPtr.Zero;
                    return true;
                }
                break;
        }

        return false;
    }

    private void UpdatePos(WinApi.POINT currentPoint) {
        WinApi.POINT delta = new WinApi.POINT(_downMousePos.X - currentPoint.X, _downMousePos.Y - currentPoint.Y);

        WinApi.RECT windowRect = _downWndRect;
        windowRect.X = windowRect.X - delta.X;
        windowRect.Y = windowRect.Y - delta.Y;

        WinApi.User32.SetWindowPos(_ownerForm.Handle, IntPtr.Zero, windowRect.X, windowRect.Y, windowRect.Width, windowRect.Height, 0);
    }

    public void PostProcessMessage(ref WindowMessage message) {

    }
}