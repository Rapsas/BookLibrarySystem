# BookLibrarySystem
Console application to manage a book library using .NET5
For this project I used a third-party-software called JSONFlatFile storage for more convenient managing of the JSON folder.
If you are thinking of making a similar app or any other small prototype project ar an API I highly recommend looking into it. https://github.com/ttu/json-flatfile-datastore 
# Functionality
- Type in command 'add' to add a new book to the library. All the information about the books is stored in a JSON file.
An example data file can be found in the release version.
- Type in command 'take' to take an existing book from the library. Specify which book is being taken, by who and for how long.
Taking the book longer than two months is not allowed. Taking more than 3 books per person is not allowed.
- Type in command 'return' to return a book. If the return is late it will display a funny message.
- Type in command 'filter' to filter the list all the books in the library by a given parameter and value.
- Type in command 'delete' to delete a book from the library system.
