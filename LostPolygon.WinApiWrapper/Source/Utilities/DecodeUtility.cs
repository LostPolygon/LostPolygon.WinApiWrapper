using System;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        public static class DecodeUtility {
            public static POINT DecodePoint(int packedPoint) {
                POINT point;

                point.X = BitUtility.LowWord(packedPoint);
                point.Y = BitUtility.HighWord(packedPoint);
                return point;
            }
        }
    }
}
