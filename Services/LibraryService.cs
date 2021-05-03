using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLibararySystem.Services.Interfaces;
using JsonFlatFileDataStore;

namespace BookLibararySystem.Services
{
    public class LibraryService : ILibraryService
    {
        // Objects used for parsing and writing data to and from data.json file
        DataStore store;
        IDocumentCollection<Book> collection;

        int globalLastId; // Used for auto incrementation of the book id

        public LibraryService()
        {
            store = new DataStore("data.json"); //Reads data from data.json
            collection = store.GetCollection<Book>("books"); // Parse the data collection called 'books'

            if (collection.AsQueryable().Count() != 0) //Handles the program behavior if the data file is empty
            {
                globalLastId = collection.AsQueryable().Last().Id;
            }
            else
            {
                globalLastId = 1;
            }
            
        }

        /// <summary>
        /// Adds the given book and updates the data file
        /// </summary>
        /// <param name="book">Book object which will be added</param>
        public Book AddBook(Book book)
        {
            book.Id = globalLastId++;
            collection.InsertOne(book);
            return book;
        }

        /// <summary>
        /// Marks a book taken and updates the data file
        /// </summary>
        /// <param name="takenID">ID of the taken book</param>
        /// <param name="takenName">Name of the person taking the book</param>
        /// <param name="retDate">Expected return date of the book</param>
        public Book TakeBook(int takenID, string takenName, DateTime retDate)
        {
            Book tkBook = collection.Find(x => x.Id == takenID).First();

            int takenBooks = collection.Find(x => x.TakenBy == takenName).Count();
            TimeSpan timeSpan = retDate - DateTime.Now;
            if(takenBooks >= 3 || timeSpan.TotalDays > 60)
            {
                throw new ArgumentException("3 books per person or maximum lend lenght rule vioalted.");
            }
            
            tkBook.TakenBy = takenName;
            tkBook.ReturnDate = retDate;
            tkBook.IsAvailable = false;

            collection.UpdateOne(takenID, tkBook);
            return tkBook;
        }

        /// <summary>
        /// Checks if the book exist in the library
        /// </summary>
        /// <param name="id">ID of the book</param>
        /// <returns>Return true if it exist and false if it does not</returns>
        public bool DoesBookExist(int id)
        {
            var tkBook = collection.Find(x => x.Id == id).FirstOrDefault();
            return tkBook != null;
        }

        /// <summary>
        /// Finds the book by ID and checks is it available for taking.
        /// Should only be called if the book exist.
        /// </summary>
        /// <param name="id">ID of the book</param>
        /// <returns>true of available, false if not</returns>
        public bool IsBookAvailable(int id)
        {
            var tkBook = collection.Find(x => x.Id == id).FirstOrDefault();
            if (tkBook == null)
            {
                return false;
            }
            return tkBook.IsAvailable;
        }

        /// <summary>
        /// Gets a list of all the books in the library
        /// </summary>
        /// <returns>List of all books</returns>
        public List<Book> GetAllBooks()
        {
            return collection.AsQueryable().ToList();   
        }

        /// <summary>
        /// Finds the returned book and updates the data file
        /// </summary>
        /// <param name="returnId">ID of the returned book</param>
        /// <returns>true if the book is late, false if it is not</returns>
        public bool ReturnBook(int returnId)
        {
            bool isLate = false;
            Book rtBook = collection.Find(x => x.Id == returnId).First();
            if (rtBook.ReturnDate < DateTime.Now)
            {
                isLate = true;
                
            }
            rtBook.TakenBy = "";
            rtBook.ReturnDate = null;
            rtBook.IsAvailable = true;
            collection.UpdateOne(returnId, rtBook);
            return isLate;
        }

        /// <summary>
        /// Deletes the specified book from the libary and updates the data file
        /// </summary>
        /// <param name="deleteId">ID of the deleted book</param>
        /// <returns>true if deleted successfully, false if not</returns>
        public bool DeleteBook(int deleteId)
        {
            return collection.DeleteOne(deleteId);
        }

        /// <summary>
        /// Filter the list of books
        /// </summary>
        /// <param name="filter">Predicate by which to filter</param>
        /// <returns>Filtered list of books</returns>
        public List<Book> Filter(Predicate<Book> filter)
        {
            
            return collection.Find(filter).ToList();
        }

        
    }
}
