using ConsoleDrawing.Commands;
using ConsoleDrawing.Models;
using System;
using System.Collections.Generic;

namespace ConsoleDrawing
{
    class Program
    {
        private static Canvas _canvas;
        private static IDictionary<string, ICommand> _commands;

        static void Main(string[] args)
        {
            InitializeCommands();

            while (true)
            {
                Console.Write("enter command: ");

                var rawInput = Console.ReadLine();
                var input = InputParser.ParseInput(rawInput);

                if (_commands.ContainsKey(input.Command))
                {
                    var command = _commands[input.Command];

                    try
                    {
                        command.Execute(input.Args);
                        Console.WriteLine(_canvas.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
                else
                    Console.WriteLine("Error: Unknown command");
            }
        }

        private static void InitializeCommands()
        {
            _commands = new Dictionary<string, ICommand>
            {
                { "C", new CreateCanvasCommand((canvas) => _canvas = canvas) },
                { "L", new DrawLineCommand(() => _canvas) },
                { "R", new DrawRectangleCommand(() => _canvas) },
                { "B", new BucketFillCommand(() => _canvas) },
                { "D", new DeleteCommand(() => _canvas) },
                { "U", new UndoCommand(() => _canvas) },
                { "Q", new ExitCommand() }
            };
        }
    }
}
