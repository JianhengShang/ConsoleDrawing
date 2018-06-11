using ConsoleDrawing.Models;
using System;

namespace ConsoleDrawing.Commands
{
    public class UndoCommand : ICommand
    {
        private Func<ICanvas> _getCanvas;

        public UndoCommand(Func<ICanvas> getCanvas)
        {
            _getCanvas = getCanvas ?? throw new ArgumentNullException(nameof(getCanvas));
        }

        public void Execute(string[] args)
        {
            if (_getCanvas() == null)
                throw new ArgumentException("No canvas exist. Please create one then try again.");

            _getCanvas().Undo();
        }
    }
}
