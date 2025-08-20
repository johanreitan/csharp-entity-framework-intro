using exercise.webapi.DTOs;
using exercise.webapi.Models;

namespace exercise.webapi.Repository
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetBooksByAuthorId(int authorId);
        public Task<Book> DeleteBook(Book b);
        public Task<IEnumerable<Book>> GetAllBooks();
        public Task<Book> GetBook(int id);

        public Task<Book> UpdateBook(Book book, BookPut model);

        public Task<Book> AddBook(Book book);
        public Task<int> GetLastId();
    }
}
