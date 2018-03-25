namespace UXI.SystemApi.Screen
{
    public class ScreenInfo
    {
        public static readonly ScreenInfo Empty = new ScreenInfo(0, 0, 0, 0);

        public ScreenInfo(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public int Left { get; }
        public int Top { get; }
        public int Width { get; }
        public int Height { get; }

        public bool Contains(int x, int y)
        {
            return x >= Left && x <= Left + Width
                && y >= Top  && y <= Top + Height;
        }

        public bool Contains(Point point)
        {
            return Contains(point.X, point.Y);
        }
    }
}
