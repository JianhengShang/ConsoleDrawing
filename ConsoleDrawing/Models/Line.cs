using System;
using System.Collections.Generic;

namespace ConsoleDrawing.Models
{
    public struct Line
    {
        public Point Origin { get; private set; }
        public Point End { get; private set; }

        public bool IsHorizontal => Origin.X == End.X;
        public bool IsVertical => Origin.Y == End.Y;

        public Line(Point origin, Point end)
        {
            Origin = origin;
            End = end;
        }

        public IEnumerable<Point> GetPoints()
        {
            if (Origin == End)
                return new List<Point> { Origin };

            var points = new List<Point>();
            if (IsHorizontal)
            {
                if (End.Y > Origin.Y)
                {
                    for (var i = Origin.Y; i <= End.Y; i++)
                        points.Add(new Point(Origin.X, i));
                }
                else
                {
                    for (var i = Origin.Y; i >= End.Y; i--)
                        points.Add(new Point(Origin.X, i));
                }
            }
            else if (IsVertical)
            {
                if (End.X > Origin.X)
                {
                    for (var i = Origin.X; i <= End.X; i++)
                        points.Add(new Point(i, Origin.Y));
                }
                else
                {
                    for (var i = Origin.X; i >= End.X; i--)
                        points.Add(new Point(i, Origin.Y));
                }
            }
            else
                throw new NotImplementedException("Only implemented for horizontal and vertical lines");

            return points;
        }
    }
}