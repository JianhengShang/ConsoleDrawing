using ConsoleDrawing.Models;
using System;

namespace ConsoleDrawing.Commands
{
    public class DrawRectangleCommand : ICommand
    {
        private Func<ICanvas> _getCanvas;

        public DrawRectangleCommand(Func<ICanvas> getCanvas)
        {
            _getCanvas = getCanvas ?? throw new ArgumentNullException(nameof(getCanvas));
        }

        public void Execute(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 4)
                throw new ArgumentException($"This command expects 4 arguments but only received {args.Length}");

            if (!uint.TryParse(args[0], out uint x1)
             || !uint.TryParse(args[1], out uint y1)
             || !uint.TryParse(args[2], out uint x2)
             || !uint.TryParse(args[3], out uint y2)
            )
                throw new ArgumentException("There is some invalid arguments. All 4 arguments should be positive integers");

            if (_getCanvas() == null)
                throw new ArgumentException("No canvas exist. Please create one then try again.");

            //adjust as coordinates are passed 1-based but the underlying canvas expects them 0-based
            var adjustedP1 = new Point(x1 - 1, y1 - 1);
            var adjustedP2 = new Point(x2 - 1, y2 - 1);

            var rectangle = new Rectangle(adjustedP1, adjustedP2);
            _getCanvas().DrawRectangle(rectangle);
        }
    }
}