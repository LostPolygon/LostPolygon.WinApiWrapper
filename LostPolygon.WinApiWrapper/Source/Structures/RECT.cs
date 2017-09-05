using System.Runtime.InteropServices;

namespace LostPolygon.WinApiWrapper {
    public partial class WinApi {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom) {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int X {
                get { return Left; }
                set {
                    Right -= (Left - value);
                    Left = value;
                }
            }

            public int Y {
                get { return Top; }
                set {
                    Bottom -= (Top - value);
                    Top = value;
                }
            }

            public int Height {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public static bool operator ==(RECT r1, RECT r2) {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2) {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r) {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj) {
                if (obj is RECT)
                    return Equals((RECT) obj);

                return false;
            }

            public override int GetHashCode() {
                unchecked {
                    int hashCode = Left;
                    hashCode = (hashCode * 397) ^ Top;
                    hashCode = (hashCode * 397) ^ Right;
                    hashCode = (hashCode * 397) ^ Bottom;
                    return hashCode;
                }
            }

            public override string ToString() {
                return string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    "{{Left={0}, Top={1}, Right={2}, Bottom={3}, Width={4}, Height={5}}}",
                    Left,
                    Top,
                    Right,
                    Bottom,
                    Width,
                    Height);
            }
        }
    }
}
