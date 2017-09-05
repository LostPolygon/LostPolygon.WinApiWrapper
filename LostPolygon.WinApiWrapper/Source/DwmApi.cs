using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        public static class DwmApi {
            #region Constants

            #endregion Constants

            #region Delegates

            #endregion Delegates

            #region Functions

            [DllImport("dwmapi.dll")]
            public static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

            [DllImport("dwmapi.dll", PreserveSig = false)]
            public static extern void DwmEnableComposition(CompositionAction uCompositionAction);

            [DllImport("Dwmapi.dll")]
            public static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

            [DllImport("dwmapi.dll", PreserveSig = true)]
            public static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attr, ref int attrValue, int attrSize);

            [DllImport("dwmapi.dll", PreserveSig = true)]
            public static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attr, out int attrValue, int attrSize);

            #endregion Functions

            #region Enums

            [Flags]
            public enum CompositionAction : uint {
                /// <summary>
                /// To enable DWM composition
                /// </summary>
                DWM_EC_DISABLECOMPOSITION = 0,

                /// <summary>
                /// To disable composition.
                /// </summary>
                DWM_EC_ENABLECOMPOSITION = 1
            }

            public enum DWMWINDOWATTRIBUTE {
                DWMWA_NCRENDERING_ENABLED = 1,      // [get] Is non-client rendering enabled/disabled
                DWMWA_NCRENDERING_POLICY,           // [set] Non-client rendering policy
                DWMWA_TRANSITIONS_FORCEDISABLED,    // [set] Potentially enable/forcibly disable transitions
                DWMWA_ALLOW_NCPAINT,                // [set] Allow contents rendered in the non-client area to be visible on the DWM-drawn frame.
                DWMWA_CAPTION_BUTTON_BOUNDS,        // [get] Bounds of the caption button area in window-relative space.
                DWMWA_NONCLIENT_RTL_LAYOUT,         // [set] Is non-client content RTL mirrored
                DWMWA_FORCE_ICONIC_REPRESENTATION,  // [set] Force this window to display iconic thumbnails.
                DWMWA_FLIP3D_POLICY,                // [set] Designates how Flip3D will treat the window.
                DWMWA_EXTENDED_FRAME_BOUNDS,        // [get] Gets the extended frame bounds rectangle in screen space
                DWMWA_HAS_ICONIC_BITMAP,            // [set] Indicates an available bitmap when there is no better thumbnail representation.
                DWMWA_DISALLOW_PEEK,                // [set] Don't invoke Peek on the window.
                DWMWA_EXCLUDED_FROM_PEEK,           // [set] LivePreview exclusion information
                DWMWA_CLOAK,                        // [set] Cloak or uncloak the window
                DWMWA_CLOAKED,                      // [get] Gets the cloaked state of the window
                DWMWA_FREEZE_REPRESENTATION,        // [set] Force this window to freeze the thumbnail without live update
                DWMWA_LAST
            }

            #endregion

            #region Structs

            [StructLayout(LayoutKind.Sequential)]
            public struct DWM_BLURBEHIND {
                public DWM_BB dwFlags;
                public bool fEnable;
                public IntPtr hRgnBlur;
                public bool fTransitionOnMaximized;

                public DWM_BLURBEHIND(bool enabled) {
                    fEnable = enabled;
                    hRgnBlur = IntPtr.Zero;
                    fTransitionOnMaximized = false;
                    dwFlags = DWM_BB.DWM_BB_ENABLE;
                }

                public bool TransitionOnMaximized {
                    get { return fTransitionOnMaximized; }
                    set {
                        fTransitionOnMaximized = value;
                        dwFlags |= DWM_BB.DWM_BB_TRANSITIONONMAXIMIZED;
                    }
                }

                [Flags]
                public enum DWM_BB {
                    DWM_BB_ENABLE = 1,
                    DWM_BB_BLURREGION = 2,
                    DWM_BB_TRANSITIONONMAXIMIZED = 4
                }
            }

            #endregion
        }
    }
}
