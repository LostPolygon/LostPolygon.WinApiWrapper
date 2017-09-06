namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        public static class TypeDecodeUtility {
            public static POINT DecodePoint(int packedPoint) {
                POINT point;

                point.X = BitUtility.LowWord(packedPoint);
                point.Y = BitUtility.HighWord(packedPoint);
                return point;
            }

            public static int EncodePoint(POINT point) {
                return (point.Y << 16) | (point.X & 0xffff);
            }
        }
    }
}
