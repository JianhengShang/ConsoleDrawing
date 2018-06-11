using ConsoleDrawing.Models;
using System;

namespace ConsoleDrawing.Commands
{
    public class CreateCanvasCommand : ICommand
    {
        private Action<Canvas> _callback;

        public CreateCanvasCommand(Action<Canvas> callback)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public void Execute(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 2)
                throw new ArgumentException($"This command expects 2 arguments but only received {args.Length}");

            if (!uint.TryParse(args[0], out uint width)
             || !uint.TryParse(args[1], out uint height))
                throw new ArgumentException("There is some invalid arguments. Both arguments should be positive integers");

            _callback.Invoke(new Canvas(width, height));
        }
    }
}