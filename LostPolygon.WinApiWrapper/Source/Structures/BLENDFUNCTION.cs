using System.Runtime.InteropServices;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION {
            //
            // currentlly defined blend operation
            //
            public const int AC_SRC_OVER = 0x00;

            //
            // currentlly defined alpha format
            //
            public const int AC_SRC_ALPHA = 0x01;

            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;

            public BLENDFUNCTION(byte op, byte flags, byte alpha, byte format) {
                BlendOp = op;
                BlendFlags = flags;
                SourceConstantAlpha = alpha;
                AlphaFormat = format;
            }
        }
    }
}
