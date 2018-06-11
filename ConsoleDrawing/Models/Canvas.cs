using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleDrawing.Models
{
    public class Canvas : ICanvas
    {
        private Stack<CanvasCell[,]> _previousStates = new Stack<CanvasCell[,]>();

        public CanvasCell[,] Cells { get; private set; }

        public int Width => Cells.GetUpperBound(0) + 1;
        public int Height => Cells.GetUpperBound(1) + 1;

        public Canvas(uint width, uint height)
        {
            Initialize(width, height);
        }

        private void Initialize(uint width, uint height)
        {
            Cells = new CanvasCell[width, height];

            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    Cells[i, j] = new CanvasCell(CanvasCellContentType.Empty, ' ');
        }

        public void DrawLine(Line line)
        {
            if (!line.IsHorizontal && !line.IsVertical)
                throw new InvalidLineException();

            if (PointIsOutOfBounds(line.Origin) || PointIsOutOfBounds(line.End))
                throw new OutOfBoundsException("This item exceeds the canvas boundaries and cannot be drawn");

            SaveState();

            InnerDrawLine(line);
        }

        private void InnerDrawLine(Line line)
        {
            var points = line.GetPoints();
            foreach (var point in points)
                DrawPoint(point);
        }

        public void DrawRectangle(Rectangle rectangle)
        {
            if (PointIsOutOfBounds(rectangle.UpperLeft)
             || PointIsOutOfBounds(rectangle.UpperRight)
             || PointIsOutOfBounds(rectangle.LowerLeft)
             || PointIsOutOfBounds(rectangle.LowerRight)
             )
                throw new OutOfBoundsException("This item exceeds the canvas boundaries and cannot be drawn");

            SaveState();

            var lines = rectangle.GetLines();

            foreach (var line in lines)
                InnerDrawLine(line);
        }

        private bool PointIsOutOfBounds(Point point)
        {
            return point.X >= Width || point.Y >= Height;
        }

        private void DrawPoint(Point point)
        {
            var cellContent = new CanvasCell(CanvasCellContentType.Line, 'x');
            Cells[point.X, point.Y] = cellContent;
        }

        public void BucketFill(Point target, char colour)
        {
            if (PointIsOutOfBounds(target))
                throw new OutOfBoundsException("The target point is out of the canvas boundaries");

            SaveState();

            InnerBucketFill(target, colour);
        }

        private void InnerBucketFill(Point target, char colour, bool deleteContentType = false)
        {

            var contentTypeToFill = Cells[target.X, target.Y].ContentType;

            var processed = new HashSet<Point>();
            var toProcess = new Queue<Point>();

            toProcess.Enqueue(target);

            Func<Point, bool> CanProcessCell = (c) => !processed.Contains(c) && !toProcess.Contains(c) && !PointIsOutOfBounds(c);

            while (toProcess.Count > 0)
            {
                var currentPoint = toProcess.Dequeue();

                processed.Add(currentPoint);

                if (Cells[currentPoint.X, currentPoint.Y].ContentType == contentTypeToFill)
                {
                    if (deleteContentType)
                        Cells[currentPoint.X, currentPoint.Y] = new CanvasCell(CanvasCellContentType.Empty, colour);
                    else
                        Cells[currentPoint.X, currentPoint.Y] = new CanvasCell(contentTypeToFill, colour);

                    var leftNeighbour = new Point(currentPoint.X - 1, currentPoint.Y);
                    if (CanProcessCell(leftNeighbour))
                        toProcess.Enqueue(leftNeighbour);

                    var rightNeighbour = new Point(currentPoint.X + 1, currentPoint.Y);
                    if (CanProcessCell(rightNeighbour))
                        toProcess.Enqueue(rightNeighbour);

                    var topNeighbour = new Point(currentPoint.X, currentPoint.Y - 1);
                    if (CanProcessCell(topNeighbour))
                        toProcess.Enqueue(topNeighbour);

                    var bottomNeighbour = new Point(currentPoint.X, currentPoint.Y + 1);
                    if (CanProcessCell(bottomNeighbour))
                        toProcess.Enqueue(bottomNeighbour);
                }
            }
        }

        public void Delete(Point target)
        {
            if (PointIsOutOfBounds(target))
                throw new OutOfBoundsException("The target point is out of the canvas boundaries");

            SaveState();

            InnerBucketFill(target, ' ', deleteContentType: true);
        }

        public void Undo()
        {
            if (_previousStates.Count > 0)
                Cells = _previousStates.Pop();
        }

        private void SaveState()
        {
            var state = new CanvasCell[Width, Height];
            Array.Copy(Cells, state, Cells.Length);
            _previousStates.Push(state);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(' ');
            for (var i = 0; i < Width; i++) sb.Append('-');

            sb.Append(Environment.NewLine);

            for (var j = 0; j < Height; j++)
            {
                sb.Append('|');
                for (var i = 0; i < Width; i++)
                {
                    sb.Append(Cells[i, j].Colour);
                }
                sb.Append('|');
                sb.Append(Environment.NewLine);
            }

            sb.Append(' ');
            for (var i = 0; i < Width; i++) sb.Append('-');

            return sb.ToString();
        }
    }

    public struct CanvasCell
    {
        public CanvasCellContentType ContentType { get; private set; }
        public char Colour { get; private set; }

        public CanvasCell(CanvasCellContentType contentType, char colour)
        {
            ContentType = contentType;
            Colour = colour;
        }
    }

    public enum CanvasCellContentType
    {
        Empty,
        Line
    }

    public class InvalidLineException : Exception
    {
        public InvalidLineException()
            : base("Only horizontal and vertical lines are currently supported")
        { }
    }

    public class OutOfBoundsException : Exception
    {
        public OutOfBoundsException(string msg)
            : base(msg)
        { }
    }
}