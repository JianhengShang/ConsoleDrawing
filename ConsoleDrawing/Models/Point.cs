namespace ConsoleDrawing.Models
{
    public struct Point
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }

        public Point(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }
}