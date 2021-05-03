using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLibararySystem.Services.Interfaces;

namespace BookLibararySystem.Services
{
    /// <summary>
    /// Contains all the methods that handle the user interface. Send data to the library service
    /// </summary>
    class DisplayService : IDisplayService
    {
        private readonly ILibraryService _libraryService;

        //Declaring strings that are used for formating.
        string header = String.Format("|{0,-3}|{1,-15}|{2,-15}|{3,-15}|{4,-15}|{5,-15}|{6,-5}|{7,-8}|{8, -12}|",
                "ID", "Name", "Author", "Category", "Language", "Published", "ISBN", "Taken By", "Return Until");
        string[] commands = { "add", "take", "return", "delete", "filter", "show all", "exit" };
        string[] filteringParameters = { "Name", "Author", "Category", "Language", "ISBN", "Taken", "Available" };

        public DisplayService(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        /// <summary>
        /// Reads data for a new book and adds it.
        /// </summary>
        public void AddBook()
        {
            Console.WriteLine("Name of the book:");
            string name = Console.ReadLine();
            Console.WriteLine("Author");
            string author = Console.ReadLine();
            Console.WriteLine("Category");
            string category = Console.ReadLine();
            Console.WriteLine("Language");
            string language = Console.ReadLine();
            Console.WriteLine("Publication date");
            DateTime pubDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("ISBN");
            int isbn = int.Parse(Console.ReadLine());

            Book book = new(name, author, category, language, pubDate, isbn);
            _libraryService.AddBook(book);
        }

        /// <summary>
        /// Reads data and deletes the specified book.
        /// </summary>
        public void DeleteBook()
        {
            Console.WriteLine("Delete book ID: ");
            int deleteId = int.Parse(Console.ReadLine());

            if (_libraryService.DeleteBook(deleteId) == false)
            {
                throw new ArgumentException("Book not found in the library.");
            }
        }

        /// <summary>
        /// Displays all of the books in the library to the console
        /// </summary>
        public void DisplayAllBooks()
        {
            Console.WriteLine(header);
            List<Book> books = _libraryService.GetAllBooks();
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        /// <summary>
        ///  Displays all of the available commands to the console.
        /// </summary>
        public void DisplayCommands()
        {
            Console.WriteLine("Available commands:");
            foreach (var command in commands)
            {
                Console.WriteLine("-{0}", command);
            }
        }

        /// <summary>
        /// Reads data then filters and displays the selected list
        /// </summary>
        public void Filter()
        {
            Console.WriteLine("Available filreting parameters:");
            foreach (var par in filteringParameters)
            {
                Console.WriteLine("-{0}", par);
            }
            Console.WriteLine("Enter the filtering parameter:");
            string parameter = Console.ReadLine().ToLower();

            Predicate<Book> filter; //This predicated will later be defined and sent to the library service for filtering

            if (parameter == "taken")
            {
                filter = x => !x.IsAvailable;
            }
            else if (parameter == "available")
            {
                filter = x => x.IsAvailable;
            }
            else
            {
                Console.WriteLine("Enter value");
                string value = Console.ReadLine().ToLower();
                switch (parameter)
                {
                    case "author":
                        filter = x => x.Author == value;
                        break;
                    case "category":
                        filter = x => x.Category == value;
                        break;
                    case "language":
                        filter = x => x.Language == value;
                        break;
                    case "isbn":
                        filter = x => x.ISBN == int.Parse(value);
                        break;
                    case "name":
                        filter = x => x.Name == value;
                        break;
                    default:
                        Console.Clear();
                        DisplayAllBooks();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Unknown filter parameter. Try again.");
                        Console.ResetColor();
                        Filter();
                        filter = x => x.Author == value;
                        break;
                }
            }

            

            List<Book> filtered = _libraryService.Filter(filter);
            if (filtered.Count != 0)
            {
                DisplaySelection(filtered);
            }
            else
            {
                Console.WriteLine("No entries found with this value. Type in new command.");
            }
        }

        /// <summary>
        /// Display only a selected list of books
        /// </summary>
        /// <param name="selection">Selected list of books</param>
        void DisplaySelection(List<Book> selection)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Showing filtered results.");
            Console.ResetColor();

            string header = String.Format("|{0,-3}|{1,-15}|{2,-15}|{3,-15}|{4,-15}|{5,-15}|{6,-5}|{7,-8}|{8, -12}|",
                "ID", "Name", "Author", "Category", "Language", "Published", "ISBN", "Taken By", "Return Until");
            Console.WriteLine(header);
            foreach (var book in selection)
            {
                Console.WriteLine(book);
            }
            Console.WriteLine("Type in a command. Type in 'h' for a list of of all commands.");
        }

        /// <summary>
        /// Reads data and returns the specified book to the library.
        /// </summary>
        public void ReturnBook()
        {
            Console.WriteLine("Returned book ID: ");
            int returnId = int.Parse(Console.ReadLine());
            if (_libraryService.DoesBookExist(returnId) == false)
            {
                throw new ArgumentException("This book does not exist.");
            }
            else if (_libraryService.IsBookAvailable(returnId) == true)
            {
                throw new ArgumentException("You can't return this book because it is not taken.");
            }
            if (_libraryService.ReturnBook(returnId))
            {
                Console.WriteLine("The book is late.\n| || \n|| |_\n Is this loss?. Press any key to continue.");
                Console.ReadKey(true);
            }
            
        }

        /// <summary>
        /// Reads data and takes out the specified book from the library.
        /// </summary>
        public void TakeBook()
        {
            Console.WriteLine("ID of taken book:");
            int takenID = int.Parse(Console.ReadLine());
            if (_libraryService.DoesBookExist(takenID) == false)
            {
                throw new ArgumentException("This book does not exist.");
            }
            else if (_libraryService.IsBookAvailable(takenID) == false)
            {
                throw new ArgumentException("This book is already taken.");
            }
            Console.WriteLine("Name of the client:");
            string takenName = Console.ReadLine();
            Console.WriteLine("Latest expected return date:");
            DateTime retDate = DateTime.Parse(Console.ReadLine());

            _libraryService.TakeBook(takenID, takenName, retDate);
        }
    }
}
