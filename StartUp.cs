using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLibararySystem.Services.Interfaces;

namespace BookLibararySystem
{
    class StartUp
    {
        private readonly IDisplayService _displayService;

        public StartUp(IDisplayService displayService)
        {
            _displayService = displayService;
        }

        public void Run()
        { 
            Console.SetWindowSize(150, Console.WindowHeight);
            bool shouldClear = true;
            string command;
            while (true)
            {
                if (shouldClear)
                {
                    Console.Clear();
                    _displayService.DisplayAllBooks();
                    Console.WriteLine("Type in a command. Type in 'h' for a list of of all commands.");
                }
                shouldClear = true;
                
                command = Console.ReadLine().ToLower();
                try
                {
                    switch (command)
                    {
                        case "h":
                            _displayService.DisplayCommands();
                            shouldClear = false;
                            break;
                        case "add":
                            Console.Clear();
                            _displayService.AddBook();
                            break;
                        case "take":
                            _displayService.TakeBook();
                            break;
                        case "return":
                            _displayService.ReturnBook();
                            break;
                        case "delete":
                            _displayService.DeleteBook();
                            break;
                        case "filter":
                            _displayService.Filter();
                            shouldClear = false;
                            break;
                        case "show all":
                            break;
                        case "exit":
                            Environment.Exit(1);
                            break;
                        default:
                            Console.WriteLine("Unkow command. Type in 'h' for a list of of all commands.");
                            shouldClear = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " Press any key to continue");
                    Console.ReadKey(true);
                }
            }

        }
    }
}
