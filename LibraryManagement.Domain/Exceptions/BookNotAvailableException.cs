namespace LibraryManagement.Domain.Exceptions
{
    public class BookNotAvailableException : Exception
    {
        public BookNotAvailableException(int bookId)
           : base($"Book with ID {bookId} is currently not available for borrowing.") { }
    }
}
