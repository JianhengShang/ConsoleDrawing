namespace ConsoleDrawing.Models
{
    public interface ICanvas
    {
        CanvasCell[,] Cells { get; }
        int Height { get; }
        int Width { get; }

        void BucketFill(Point target, char colour);

        void Delete(Point target);

        void DrawLine(Line line);

        void DrawRectangle(Rectangle rectangle);

        void Undo();
    }
}