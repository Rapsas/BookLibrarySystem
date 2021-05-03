using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibararySystem.Services.Interfaces
{
    interface ILibraryService
    {
        List<Book> GetAllBooks();
        Book AddBook(Book book);
        Book TakeBook(int takenID, string takenName, DateTime retDate);
        bool DoesBookExist(int id);
        bool IsBookAvailable(int id);
        bool ReturnBook(int returnId);
        bool DeleteBook(int id);
        List<Book> Filter(Predicate<Book> filter);
    }
}
