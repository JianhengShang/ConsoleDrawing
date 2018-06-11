using ConsoleDrawing.Models;
using System;

namespace ConsoleDrawing.Commands
{
    public class BucketFillCommand : ICommand
    {
        private Func<ICanvas> _getCanvas;

        public BucketFillCommand(Func<ICanvas> getCanvas)
        {
            _getCanvas = getCanvas ?? throw new ArgumentNullException(nameof(getCanvas));
        }

        public void Execute(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 3)
                throw new ArgumentException($"This command expects 3 arguments but only received {args.Length}");

            if (!uint.TryParse(args[0], out uint x)
             || !uint.TryParse(args[1], out uint y)
             || !char.TryParse(args[2], out char colour))
                throw new ArgumentException("There are some invalid arguments. The 2 first arguments should be positive integer and the last one should be an alphanumerical character");

            if (_getCanvas() == null)
                throw new ArgumentException("No canvas exist. Please create one then try again.");

            //adjust as coordinates are passed 1-based but the underlying canvas expects them 0-based
            var adjustedTarget = new Point(x - 1, y - 1);

            _getCanvas().BucketFill(adjustedTarget, colour);
        }
    }
}