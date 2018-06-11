using ConsoleDrawing.Models;
using System;

namespace ConsoleDrawing.Commands
{
    public class DeleteCommand : ICommand
    {
        private Func<ICanvas> _getCanvas;

        public DeleteCommand(Func<ICanvas> getCanvas)
        {
            _getCanvas = getCanvas ?? throw new ArgumentNullException(nameof(getCanvas));
        }

        public void Execute(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 2)
                throw new ArgumentException($"This command expects 2 arguments but only received {args.Length}");

            if (!uint.TryParse(args[0], out uint x) || !uint.TryParse(args[1], out uint y))
                throw new ArgumentException("There are some invalid arguments. The 2 arguments should be positive integers");

            if (_getCanvas == null || _getCanvas() == null)
                throw new ArgumentException("No canvas exist. Please create one then try again.");

            //adjust as coordinates are passed 1-based but the underlying canvas expects them 0-based
            _getCanvas().Delete(new Point(x - 1, y - 1));
        }
    }
}