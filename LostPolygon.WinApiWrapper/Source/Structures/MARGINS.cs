using System.Runtime.InteropServices;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }
    }
}
