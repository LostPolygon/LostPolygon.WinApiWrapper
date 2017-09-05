using System.Runtime.InteropServices;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int X;
            public int Y;

            public POINT(int x, int y) {
                X = x;
                Y = y;
            }
        }
    }
}
