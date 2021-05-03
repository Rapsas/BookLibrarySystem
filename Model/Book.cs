using JsonFlatFileDataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookLibararySystem
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ISBN { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string TakenBy { get; set; }
        public bool IsAvailable { get; set; }

        public Book(string name, string author, string category, string language, DateTime publicationDate, int iSBN, DateTime? returnDate = null, string takenBy = "")
        {
            Id = 1;
            Name = name;
            Author = author;
            Category = category;
            Language = language;
            PublicationDate = publicationDate;
            ISBN = iSBN;
            ReturnDate = returnDate;
            TakenBy = takenBy;
            IsAvailable = true;
        }

        public override string ToString()
        {
            string shortReturnDate;
            if (ReturnDate.HasValue)
            {
                shortReturnDate = ReturnDate.Value.ToShortDateString();
            }
            else
            {
                shortReturnDate = "";
            }
            return String.Format("|{0,-3}|{1,-15}|{2,-15}|{3,-15}|{4,-15}|{5,-15}|{6,-5}|{7,-8}|{8, -12}|",
                Id, Name, Author, Category, Language, PublicationDate.ToShortDateString(), ISBN, TakenBy, shortReturnDate);
        }
    }
}
