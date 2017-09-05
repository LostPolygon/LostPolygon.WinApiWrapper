using System.Runtime.InteropServices;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTS {
            public short X;
            public short Y;

            public POINTS(short x, short y) {
                X = x;
                Y = y;
            }
        }
    }
}
