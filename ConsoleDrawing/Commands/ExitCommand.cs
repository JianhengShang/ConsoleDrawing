using System;

namespace ConsoleDrawing.Commands
{
    public class ExitCommand : ICommand
    {
        public void Execute(string[] args)
        {
            Environment.Exit(0);
        }
    }
}