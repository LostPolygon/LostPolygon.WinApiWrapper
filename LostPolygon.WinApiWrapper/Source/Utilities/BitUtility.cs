namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        public static class BitUtility {
            public static int LowWord(int number) {
                return unchecked((short) number);
            }

            public static int HighWord(int number) {
                return unchecked((short) ((long) number >> 16));
            }
        }
    }
}
