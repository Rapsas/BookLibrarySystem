using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibararySystem.Services.Interfaces
{
    interface IDisplayService
    {
        void DisplayCommands();
        void AddBook();
        void TakeBook();
        void ReturnBook();
        void DeleteBook();
        void Filter();
        void DisplayAllBooks();
    }
}
